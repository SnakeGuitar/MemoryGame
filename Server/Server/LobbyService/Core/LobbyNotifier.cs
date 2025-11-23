using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.LobbyService.Core
{
    internal class LobbyNotifier
    {
        private readonly Action<string> _onDisconnectDetected;

        public LobbyNotifier(Action<string> onDisconnectDetected)
        {
            _onDisconnectDetected = onDisconnectDetected;
        }

        public void BroadcastMessage(Lobby lobby, string message, bool isNotification, string senderName = null)
        {
            if (lobby == null)
            {
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
                        c.Callback.ReceiveChatMessage(actualSender, message, isNotification);
                    }
                    catch (Exception)
                    {
                        _onDisconnectDetected?.Invoke(c.SessionId);
                    }
                });
            }
        }

        public void NotifyJoin(Lobby lobby, string playerName)
        {
            if (lobby == null)
            {
                return;
            }
            BroadcastPlayerList(lobby);
            BroadcastMessage(lobby, $"{playerName} has joined the lobby.", true);
        }

        public void NotifyLeave(Lobby lobby, string playerName)
        {
            if (lobby == null)
            {
                return;
            }
            BroadcastPlayerList(lobby);
            BroadcastMessage(lobby, $"{playerName} has left the lobby.", true);
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

            foreach (var client in lobby.Clients.Values.ToArray())
            {
                var c = client;
                Task.Run(() =>
                {
                    try
                    {
                        c.Callback.UpdatePlayerList(playerList);
                    }
                    catch (Exception)
                    {
                        _onDisconnectDetected?.Invoke(c.SessionId);
                    }
                });
            }
        }
    }
}
