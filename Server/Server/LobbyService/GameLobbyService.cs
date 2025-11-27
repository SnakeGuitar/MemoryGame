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
using log4net.Core;

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
        private readonly ILoggerManager _logger;
        public GameLobbyService(ISecurityService securityService,
            ISessionManager sessionManager,
            ILobbyCallbackProvider callbackProvider,
            IGameLobbyServiceValidator validator,
            ILoggerManager logger)
        {
            _sessionManager = sessionManager;
            _securityService = securityService;
            _callbackProvider = callbackProvider;
            _validator = validator;

            _logger = logger;
            _stateManager = new LobbyStateManager(_logger);
            _notifier = new LobbyNotifier(sessionId => HandleDisconnection(sessionId));
            
        }

        public GameLobbyService() : this(
            new SecurityService(),
            new SessionManager(),
            new WcfLobbyCallbackProvider(),
            new GameLobbyServiceValidator(),
            new Logger(typeof(GameLobbyService)))
        {
        }

        public bool JoinLobby(string token, string gameCode, bool isGuest, string guestName = null)
        {
            if (!_validator.IsValidGameCode(gameCode))
            {
                _logger.LogWarn($"Invalid game code attempted: {gameCode}");
                return false;
            }

            if (_stateManager.IsGameStarted(gameCode))
            {
                _logger.LogWarn($"Attempt to join started game: {gameCode}");
                return false;
            }

            var callback = _callbackProvider.GetCallback();
            string sessionId = OperationContext.Current?.SessionId ?? Guid.NewGuid().ToString();

            if (_stateManager.IsInLobby(sessionId))
            {
                _logger.LogWarn($"Session {sessionId} attempted to join multiple lobbies.");
                return false;
            }

            string playerName = ResolvePlayerName(token, isGuest, guestName);
            if (string.IsNullOrEmpty(playerName))
            {
                _logger.LogWarn($"Failed to resolve player name for session {sessionId}.");
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

            _logger.LogInfo($"Player {playerName} (Session: {sessionId}) is attempting to join lobby {gameCode}");

            if (_stateManager.TryJoinLobby(gameCode, newClient, out var lobby))
            {
                SubscribeToDisconnect(callback, sessionId);
                _notifier.NotifyJoin(lobby, gameCode);
                _logger.LogInfo($"Player {playerName} joined lobby {gameCode} (Session: {sessionId})");
                return true;
            }
            _logger.LogWarn($"Failed to add player {playerName} to lobby {gameCode} (Session: {sessionId})");
            return false;
        }

        public void LeaveLobby()
        {
            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId != null)
            {
                HandleDisconnection(sessionId);
                _logger.LogInfo($"Session {sessionId} has left the lobby.");
            }
        }

        public void SendChatMessage(string message)
        {
            if (!_validator.IsValidChatMessage(message))
            {
                _logger.LogWarn("Invalid chat message attempted.");
                return;
            }

            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId == null)
            {
                _logger.LogWarn("Chat message attempted with null session ID.");
                return;
            }

            var lobby = _stateManager.GetLobbyBySession(sessionId);
            if (lobby != null)
            {
                var client = lobby.Clients.Values.FirstOrDefault(c => c.SessionId == sessionId);
                if (client != null)
                {
                    _notifier.BroadcastMessage(lobby, message, false, client.Name);
                    _logger.LogInfo($"Chat message from {client.Name} in lobby {lobby.GameCode}: {message}");
                }
            }
        }

        public void StartGame(GameSettings settings)
        {
            var sessionId = OperationContext.Current?.SessionId;
            if (sessionId == null)
            {
                _logger.LogWarn($"{nameof(StartGame)}");
                return;
            }

            if (!_stateManager.TryStartGame(sessionId, settings, out var gameCode))
            {
                _logger.LogInfo($"Starting game {sessionId}");
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
                _logger.LogWarn($"{nameof(FlipCard)} called with null session ID.");
                return;
            }

            var gameManager = _stateManager.GetGameManager(sessionId);
            var playerId = _stateManager.GetPlayerId(sessionId);

            if (gameManager != null && playerId != null)
            {
                if (_validator.IsValidCardIndex(cardIndex, 100))
                {
                    gameManager.HandleFlipCard(playerId, cardIndex);
                    _logger.LogInfo($"Player {playerId} flipped card {cardIndex}.");
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
                _logger.LogInfo($"Client {client.Name} disconnected from lobby {gameCode} (Session: {sessionId})");
            }
        }

        private void SubscribeToDisconnect(IGameLobbyCallback callback, string sessionId)
        {
            if (callback is ICommunicationObject commObject)
            {
                commObject.Faulted += (s, e) => HandleDisconnection(sessionId);
                commObject.Closed += (s, e) => HandleDisconnection(sessionId);
                _logger.LogInfo($"Subscribed to disconnect events for session {sessionId}.");
            }
        }

        private string ResolvePlayerName(string token, bool isGuest, string guestName)
        {
            if (isGuest)
            {
                _logger.LogInfo("Resolving guest player name.");
                return _validator.IsValidGuestName(guestName) ? guestName.Trim() : null;
            }
            int? userId = _sessionManager.GetUserIdFromToken(token);

            _logger.LogInfo(userId.HasValue
                ? $"Resolved player name for user ID {userId.Value}."
                : "Failed to resolve player name from token.");
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
