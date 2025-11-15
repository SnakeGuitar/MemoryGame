using Server.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server.LobbyService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class GameLobbyService : IGameLobbyService
    {
        private readonly ConcurrentDictionary<string, Lobby> _lobbies = new ConcurrentDictionary<string, Lobby>();
        private readonly SessionCreation _sessionValidator = new SessionCreation();
        public bool JoinLobby(string token, string gameCode, bool isGuest, string guestName = null)
        {
            if (string.IsNullOrEmpty(gameCode) || gameCode.Length > 6 || !gameCode.All(char.IsDigit))
            {
                return false;
            }

            gameCode = gameCode.ToUpperInvariant();

            var callback = OperationContext.Current.GetCallbackChannel<IGameLobbyCallback>();
            string playerName;

            if (!isGuest)
            {
                int? userId = _sessionValidator.GetUserIdFromToken(token);
                if (userId == null)
                {
                    return false;
                }
                playerName = UserServiceUtilities.GetUsernameById(userId.Value);
                if (string.IsNullOrEmpty(playerName))
                {
                    return false;
                }
            }
            else
            {
                playerName = guestName?.Trim();
                if (string.IsNullOrEmpty(playerName) || playerName.Length > 30)
                {
                    return false;
                }
            }
            var lobby = _lobbies.GetOrAdd(gameCode, code => new Lobby
            {
                GameCode = code,
                CreatedAt = DateTime.UtcNow
            });

            if (lobby.Clients.Count >= 4)
            {
                return false;
            }

            var clientId = Guid.NewGuid().ToString("N");
            var newClient = new LobbyClient
            {
                Id = clientId,
                Name = playerName,
                IsGuest = isGuest,
                Callback = callback,
                JoinedAt = DateTime.UtcNow
            };

            if (!lobby.Clients.TryAdd(clientId, newClient))
            {
                return false;
            }

            BroadcastPlayerList(gameCode);
            BroadcastMessage(gameCode, $"{playerName} has joined the lobby.", true);
            return true;
        }

        public void LeaveLobby()
        {
            try
            {
                var callback = OperationContext.Current.GetCallbackChannel<IGameLobbyCallback>();

                if (!TryFindClientByCallback(callback, out var lobby, out var gameCode, out var client))
                {
                    return;
                }
                lobby.Clients.TryRemove(client.Id, out _);

                if (lobby.Clients.IsEmpty)
                {
                    _lobbies.TryRemove(gameCode, out _);
                }
                BroadcastPlayerList(gameCode);
                BroadcastMessage(gameCode, $"{client.Name} has left the lobby.", true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LeaveLobby Error: {ex.Message}");
            }
        }

        public void SendChatMessage(string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message) || message.Length > 500)
                {
                    return;
                }

                var callback = OperationContext.Current.GetCallbackChannel<IGameLobbyCallback>();

                if (!TryFindClientByCallback(callback, out var lobby, out var gameCode, out var sender))
                {
                    throw new FaultException("You are not in a lobby.");
                }

                BroadcastMessage(gameCode, message, false, excludeId: sender.Id, senderName: sender.Name);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SendChatMessage Error: {ex.Message}");
            }
        }

        private void BroadcastMessage(
            string gameCode,
            string message,
            bool isNotification,
            string excludeId = null,
            string senderName = null)
        {
            if (!_lobbies.TryGetValue(gameCode, out var lobby))
            {
                return;
            }

            var tasks = new List<Task>();
            foreach (var client in lobby.Clients.Values)
            {
                if (client.Id == excludeId)
                {
                    continue;
                }
                var c = client;

                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        c.Callback.ReceiveChatMessage(isNotification ? "System" : c.Name, message, isNotification);
                        c.Callback.ReceiveChatMessage(senderName, message, isNotification);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Client disconnected during broadcast: {ex.Message}");
                        lobby.Clients.TryRemove(c.Id, out _);
                    }
                }));
            }

            _ = Task.WhenAll(tasks);
        }

        private void BroadcastPlayerList(string gameCode)
        {
            if (!_lobbies.TryGetValue(gameCode, out var lobby))
            {
                return;
            }

            var playerList = lobby.Clients.Values
                .Select(c => new LobbyPlayerInfo
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsGuest = c.IsGuest,
                    JoinedAt = c.JoinedAt
                })
                .ToArray();

            var tasks = new List<Task>();
            foreach (var client in lobby.Clients.Values)
            {
                var c = client;
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        c.Callback.UpdatePlayerList(playerList);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Client disconnected during player list update: {ex.Message}");
                        lobby.Clients.TryRemove(c.Id, out _);
                    }
                }));
            }
        }

        private bool TryFindClientByCallback(
            IGameLobbyCallback callback,
            out Lobby lobby,
            out string gameCode,
            out LobbyClient client)
        {
            lobby = null;
            gameCode = null;
            client = null;

            foreach (var kvp in _lobbies)
            {
                var foundClient = kvp.Value.Clients.Values.FirstOrDefault(c => c.Callback == callback);
                if (foundClient != null)
                {
                    lobby = kvp.Value;
                    gameCode = kvp.Key;
                    client = foundClient;
                    return true;
                }
            }
            return false;
        }

        private class LobbyClient
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsGuest { get; set; }
            public IGameLobbyCallback Callback { get; set; }
            public DateTime JoinedAt { get; set; }
        }

        private class Lobby
        {
            public string GameCode { get; set; }
            public ConcurrentDictionary<string, LobbyClient> Clients { get; set; } = new ConcurrentDictionary<string, LobbyClient>();
            public DateTime CreatedAt { get; set; }
        }
    }
}
