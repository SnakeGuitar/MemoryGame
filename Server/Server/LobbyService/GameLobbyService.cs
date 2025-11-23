using Server.GameService;
using Server.SessionService;
using Server.Shared;
using Server.Validator;
using Server.LobbyService.Core;
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
        private readonly LobbyStateManager _stateManager;
        private readonly LobbyNotifier _notifier;

        private readonly ISessionManager _sessionManager;
        private readonly ISecurityService _securityService;
        private readonly ILobbyCallbackProvider _callbackProvider;
        private readonly IGameLobbyServiceValidator _validator;
        public GameLobbyService(ISecurityService securityService,
            ISessionManager sessionManager,
            ILobbyCallbackProvider callbackProvider,
            IGameLobbyServiceValidator validator)
        {
            _sessionManager = sessionManager;
            _securityService = securityService;
            _callbackProvider = callbackProvider;
            _validator = validator;

            _stateManager = new LobbyStateManager();
            _notifier = new LobbyNotifier(sessionId => HandleDisconnection(sessionId));
        }

        public GameLobbyService() : this(
            new SecurityService(),
            new SessionManager(),
            new WcfLobbyCallbackProvider(),
            new GameLobbyServiceValidator())
        {
        }

        public bool JoinLobby(string token, string gameCode, bool isGuest, string guestName = null)
        {
            if (!_validator.IsValidGameCode(gameCode))
            {
                return false;
            }

            if (_stateManager.IsGameStarted(gameCode))
            {
                return false;
            }

            var callback = _callbackProvider.GetCallback();
            string sessionId = OperationContext.Current?.SessionId ?? Guid.NewGuid().ToString();

            if (_stateManager.IsInLobby(sessionId))
            {
                return false;
            }

            string playerName = ResolvePlayerName(token, isGuest, guestName);
            if (string.IsNullOrEmpty(playerName))
            {
                return false;
            }

            var newClient = new LobbyClient
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = playerName,
                IsGuest = isGuest,
                Callback = callback,
                JoinedAt = DateTime.UtcNow,
                SessionId = sessionId
            };

            if (_stateManager.TryJoinLobby(gameCode, newClient, out var lobby))
            {
                SubscribeToDisconnect(callback, sessionId);
                _notifier.NotifyJoin(lobby, gameCode);
                return true;
            }
            return false;
        }

        public void LeaveLobby()
        {
            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId != null)
            {
                HandleDisconnection(sessionId);
            }
        }

        public void SendChatMessage(string message)
        {
            if (!_validator.IsValidChatMessage(message))
            {
                return;
            }

            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId == null)
            {
                return;
            }

            var lobby = _stateManager.GetLobbyBySession(sessionId);
            if (lobby != null)
            {
                var client = lobby.Clients.Values.FirstOrDefault(c => c.SessionId == sessionId);
                if (client != null)
                {
                    _notifier.BroadcastMessage(lobby, message, false, client.Name);
                }
            }
        }

        public void StartGame(GameSettings settings)
        {
            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId == null)
            {
                return;
            }

            if (!_stateManager.TryStartGame(sessionId, settings, out var gameCode))
            {
                throw new FaultException("Cannot start game. Either not in lobby or game already started.");
            }

            var lobby = _stateManager.GetLobbyBySession(sessionId);
            _notifier.BroadcastMessage(lobby, "Game has started!", true);

            // TODO
            // NOTIFY WINDOW CHANGE IN CLIENT
        }

        public void FlipCard(int cardIndex)
        {
            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId == null)
            {
                return;
            }

            var gameManager = _stateManager.GetGameManager(sessionId);
            var playerId = _stateManager.GetPlayerId(sessionId);

            if (gameManager != null && playerId != null)
            {
                if (_validator.IsValidCardIndex(cardIndex, 100))
                {
                    gameManager.HandleFlipCard(playerId, cardIndex);
                }
            }
        }

        private void HandleDisconnection(string sessionId)
        {
            var client = _stateManager.RemoveClient(sessionId, out var gameCode);

            if (client != null)
            {
                var lobby = _stateManager.GetLobbyBySession(sessionId);

                _notifier.NotifyLeave(lobby, client.Name);
            }
        }

        private void SubscribeToDisconnect(IGameLobbyCallback callback, string sessionId)
        {
            if (callback is ICommunicationObject commObject)
            {
                commObject.Faulted += (s, e) => HandleDisconnection(sessionId);
                commObject.Closed += (s, e) => HandleDisconnection(sessionId);
            }
        }

        private string ResolvePlayerName(string token, bool isGuest, string guestName)
        {
            if (isGuest)
            {
                return _validator.IsValidGuestName(guestName) ? guestName.Trim() : null;
            }
            int? userId = _sessionManager.GetUserIdFromToken(token);
            return userId.HasValue ? _securityService.GetUsernameById(userId.Value) : null;
        }

        private class Lobby
        {
            public string GameCode { get; set; }
            public ConcurrentDictionary<string, LobbyClient> Clients { get; set; } = new ConcurrentDictionary<string, LobbyClient>();
            public DateTime CreatedAt { get; set; }
        }
    }
}
