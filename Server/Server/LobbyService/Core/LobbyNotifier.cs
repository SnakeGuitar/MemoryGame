using log4net.Core;
using Server.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.LobbyService.Core
{
    public class LobbyNotifier
    {
        private readonly Action<string> _onDisconnectDetected;
        private readonly Shared.ILoggerManager _logger;

        public LobbyNotifier(Action<string> onDisconnectDetected, ILoggerManager logger)
        {
            _onDisconnectDetected = onDisconnectDetected;
            _logger = logger;
        }

        public LobbyNotifier(Action<string> onDisconnectDetected) : this(
            onDisconnectDetected, 
            new Logger(typeof(LobbyNotifier)))
        {
        }

        // viola estándar
        public void BroadcastMessage(Lobby lobby, string message, bool isNotification, string senderName = null)
        {
            if (lobby == null)
            {
                _logger.LogWarn("Attempted to broadcast message to a null lobby.");
                return;
            }

            var clients = lobby.Clients.Values.ToArray();
            string actualSender = isNotification ? "System" : (senderName ?? "Unknown");

            foreach (var client in clients)
            {
                var c = client;
                Task.Run(() =>
                {
                    try
                    {
                        _logger.LogInfo($"Sending message to {c.Name} ({c.Id}): [{actualSender}] {message}");
                        c.Callback.ReceiveChatMessage(actualSender, message, isNotification);
                    }
                    catch (Exception)
                    {
                        _logger.LogWarn($"Failed to send message to {c.Name} ({c.Id}). Assuming disconnected.");
                        _onDisconnectDetected?.Invoke(c.SessionId);
                    }
                });
            }
        }

        public void NotifyJoin(Lobby lobby, string playerName)
        {
            if (lobby == null)
            {
                _logger.LogWarn("Attempted to notify join on a null lobby.");
                return;
            }
            BroadcastPlayerList(lobby);
            BroadcastMessage(lobby, $"{playerName} has joined the lobby.", true);
            _logger.LogInfo($"{playerName} has joined the lobby {lobby.GameCode}.");
        }

        public void NotifyLeave(Lobby lobby, string playerName)
        {
            if (lobby == null)
            {
                _logger.LogWarn("Attempted to notify leave on a null lobby.");
                return;
            }
            BroadcastPlayerList(lobby);
            BroadcastMessage(lobby, $"{playerName} has left the lobby.", true);
            _logger.LogInfo($"{playerName} has left the lobby {lobby.GameCode}.");
        }

        private void BroadcastPlayerList(Lobby lobby)
        {
            var playerList = lobby.Clients.Values.Select(c => new LobbyPlayerInfo
            {
                Id = c.Id,
                Name = c.Name,
                IsGuest = c.IsGuest,
                JoinedAt = c.JoinedAt
            }).ToArray();
            _logger.LogInfo($"Broadcasting updated player list to lobby {lobby.GameCode}.");

            foreach (var client in lobby.Clients.Values.ToArray())
            {
                var c = client;
                Task.Run(() =>
                {
                    try
                    {
                        _logger.LogInfo($"Updating player list for {c.Name} ({c.Id}).");
                        c.Callback.UpdatePlayerList(playerList);
                    }
                    catch (Exception)
                    {
                        _logger.LogWarn($"Failed to update player list for {c.Name} ({c.Id}). Assuming disconnected.");
                        _onDisconnectDetected?.Invoke(c.SessionId);
                    }
                });
            }
        }
    }
}
