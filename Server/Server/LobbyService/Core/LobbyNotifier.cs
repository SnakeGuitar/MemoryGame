using Server.GameService;
using Server.SessionService;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Server.LobbyService.Core
{
    public class LobbyNotifier
    {
        private readonly Action<string> _disconnectCallback;
        private readonly ILoggerManager _logger;

        public LobbyNotifier(Action<string> disconnectCallback, ILoggerManager logger)
        {
            _disconnectCallback = disconnectCallback;
            _logger = logger;
        }

        public void BroadcastMessage(Lobby lobby, string message, bool isNotification, string senderName)
        {
            if (lobby == null)
            { 
                return; 
            }

            foreach (var client in lobby.Clients.Values.ToList())
            {
                try
                {
                    if (client.Callback is ICommunicationObject commObj && commObj.State != CommunicationState.Opened)
                    {
                        _logger.LogWarn($"Client {client.Name} in lobby {lobby.GameCode} has a closed communication channel.");
                        throw new CommunicationException("Channel closed");
                    }

                    client.Callback.ReceiveChatMessage(senderName, message, isNotification);
                }
                catch (CommunicationException) 
                { 
                    HandleFailedClient(client);
                    _logger.LogWarn($"CommunicationException when sending message to client {client.Name} in lobby {lobby.GameCode}.");
                }
                catch (TimeoutException)
                { 
                    HandleFailedClient(client); 
                    _logger.LogWarn($"TimeoutException when sending message to client {client.Name} in lobby {lobby.GameCode}.");
                }
                catch (Exception)
                {
                _logger.LogError($"Unexpected error when sending message to client {client.Name} in lobby {lobby.GameCode}.");
                }
            }
        }

        public void NotifyJoin(Lobby lobby, string newPlayerName)
        {
            if (lobby == null) return;

            var allPlayers = lobby.Clients.Values
                .Select(c => new LobbyPlayerInfo { Name = c.Name })
                .ToArray();

            var newClientObj = lobby.Clients.Values.FirstOrDefault(c => c.Name == newPlayerName);
            bool isGuest = newClientObj != null && newClientObj.IsGuest;

            foreach (var client in lobby.Clients.Values.ToList())
            {
                try
                {
                    if (client.Name != newPlayerName)
                    {
                        client.Callback.PlayerJoined(newPlayerName, isGuest);
                    }
                    client.Callback.UpdatePlayerList(allPlayers);
                }
                catch (CommunicationException) 
                { 
                    _logger.LogWarn($"CommunicationException when notifying join to client {client.Name} in lobby {lobby.GameCode}.");
                    HandleFailedClient(client); 
                }
                catch (TimeoutException) 
                { 
                    _logger.LogWarn($"TimeoutException when notifying join to client {client.Name} in lobby {lobby.GameCode}.");
                    HandleFailedClient(client); 
                }
            }
        }

        public void NotifyLeave(Lobby lobby, string leftPlayerName)
        {
            if (lobby == null) return;

            var remainingPlayers = lobby.Clients.Values
                .Where(c => c.Name != leftPlayerName)
                .Select(c => new LobbyPlayerInfo { Name = c.Name })
                .ToArray();

            foreach (var client in lobby.Clients.Values.ToList())
            {
                if (client.Name == leftPlayerName) continue;

                try
                {
                    client.Callback.PlayerLeft(leftPlayerName);
                    client.Callback.UpdatePlayerList(remainingPlayers);
                }
                catch (CommunicationException) 
                { 
                    HandleFailedClient(client); 
                }
                catch (TimeoutException) 
                { 
                    HandleFailedClient(client); 
                }
            }
        }

        /// <summary>
        /// Auxiliar method for starting the game by sending the cards.
        /// </summary>
        public void NotifyGameStarted(Lobby lobby, List<CardInfo> deck)
        {
            if (lobby == null)
            {
                return;
            }

            foreach (var client in lobby.Clients.Values.ToList())
            {
                try
                {
                    client.Callback.GameStarted(deck);
                }
                catch (CommunicationException) 
                { 
                    _logger.LogWarn($"CommunicationException when notifying game start to client {client.Name} in lobby {lobby.GameCode}.");
                    HandleFailedClient(client); 
                }
                catch (TimeoutException) 
                { 
                    _logger.LogWarn($"TimeoutException when notifying game start to client {client.Name} in lobby {lobby.GameCode}.");
                    HandleFailedClient(client); 
                }
            }
        }

        private void HandleFailedClient(LobbyClient client)
        {
            _disconnectCallback?.Invoke(client.SessionId);
        }
    }
}