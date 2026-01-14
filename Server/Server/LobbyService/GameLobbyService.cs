using Server.GameService;
using Server.LobbyService.Core;
using Server.SessionService;
using Server.Shared;
using Server.Validator;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Net.Mail;

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
        private readonly IDbContextFactory _dbFactory;

        public GameLobbyService(ISecurityService securityService,
            ISessionManager sessionManager,
            ILobbyCallbackProvider callbackProvider,
            IGameLobbyServiceValidator validator,
            ILoggerManager logger,
            IDbContextFactory dbFactory)
        {
            _sessionManager = sessionManager;
            _securityService = securityService;
            _callbackProvider = callbackProvider;
            _validator = validator;

            _logger = logger;
            _stateManager = new LobbyStateManager(_logger);
            _notifier = new LobbyNotifier(sessionId => HandleDisconnection(sessionId), _logger);
            _dbFactory = dbFactory;
        }

        public GameLobbyService() : this(
            new SecurityService(),
            new SessionManager(),
            new WcfLobbyCallbackProvider(),
            new GameLobbyServiceValidator(),
            new Logger(typeof(GameLobbyService)),
            new DbContextFactory())
        {
        }

        public bool JoinLobby(string token, string gameCode, bool isGuest, string guestName = null)
        {
            
            return ServerExceptionManager.SafeExecute(() =>
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

                var client = PrepareClient(token, isGuest, guestName);
                if (client == null)
                {
                    return false;
                }

                if (_stateManager.TryJoinLobby(gameCode, client, out var lobby))
                {
                    _logger.LogInfo($"Client {client.Name} joined lobby {gameCode}.");
                    _notifier.NotifyJoin(lobby, client.Name);
                    SubscribeToDisconnect(client.Callback, client.SessionId);
                    return true;
                }
                return false;
            }, _logger, nameof(JoinLobby));
        }

        public bool CreateLobby(string token, string gameCode, bool isPublic)
        {
            return ServerExceptionManager.SafeExecute(() =>
            {
                if (!_validator.IsValidGameCode(gameCode))
                {
                    return false;
                }

                var client = PrepareClient(token, false, null);
                if (client == null)
                {
                    return false;
                }

                if (_stateManager.TryCreateLobby(gameCode, client, out var lobby))
                {
                    lobby.IsPublic = isPublic;
                    _logger.LogInfo($"Client {client.Name} created lobby {gameCode}.");
                    SubscribeToDisconnect(client.Callback, client.SessionId);
                    return true;
                }

                return false;
            }, _logger, nameof(CreateLobby));
        }

        public void LeaveLobby()
        {
            ServerExceptionManager.SafeExecute(() =>
            {
                var sessionId = _callbackProvider.GetSessionId();
                if (sessionId != null)
                {
                    HandleDisconnection(sessionId);
                    _logger.LogInfo($"Session {sessionId} has left the lobby.");
                }
            }, _logger, nameof(LeaveLobby));
        }

        public void SendChatMessage(string message)
        {
            ServerExceptionManager.SafeExecute(() =>
            {
                if (!_validator.IsValidChatMessage(message))
                {
                    _logger.LogWarn("Invalid chat message attempted.");
                    return;
                }

                var sessionId = _callbackProvider.GetSessionId();
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
            }, _logger, nameof(SendChatMessage));
        }

        public void StartGame(GameSettings settings)
        {
            ServerExceptionManager.SafeExecute(() =>
            {
                var sessionId = _callbackProvider.GetSessionId();
                if (sessionId == null)
                {
                    _logger.LogWarn($"{nameof(StartGame)} called with null session ID.");
                    return;
                }

                var lobby = _stateManager.GetLobbyBySession(sessionId);
                if (lobby == null)
                {
                    _logger.LogWarn($"Cannot start game: Session {sessionId} not in lobby.");
                    throw new FaultException("Not in a lobby.");
                }

                if (!_stateManager.TryStartGame(sessionId, settings, out var gameCode))
                {
                    _logger.LogInfo($"Failed to start game {gameCode}");
                    throw new FaultException("Cannot start game. Either not in lobby or game already started.");
                }

                var gameManager = _stateManager.GetGameManager(sessionId);
                if (gameManager != null)
                {
                    gameManager.GameEnded += (winnerId, scores) => OnGameEnded(lobby, winnerId, scores);
                }

                _notifier.BroadcastMessage(lobby, "Game has started!", true, "System");
            }, _logger, nameof(StartGame));
        }

        private void OnGameEnded(Lobby lobby, string winnerClientId, Dictionary<string, int> scores)
        {
            Task.Run(() =>
            {
                ServerExceptionManager.SafeExecute(() =>
                {
                    using (var db = _dbFactory.Create())
                    {
                        var match = new match
                        {
                            startDateTime = DateTime.UtcNow,
                            endDateTime = DateTime.UtcNow
                        };
                        db.match.Add(match);
                        db.SaveChanges();

                        int? winnerUserId = null;
                        if (lobby.Clients.TryGetValue(winnerClientId, out var winnerClient))
                        {
                            winnerUserId = winnerClient.UserId;
                        }

                        foreach (var kvp in scores)
                        {
                            string clientId = kvp.Key;
                            int score = kvp.Value;

                            if (lobby.Clients.TryGetValue(clientId, out var client) && client.UserId.HasValue)
                            {
                                var history = new matchHistory
                                    {
                                        matchId = match.matchId,
                                        userId = client.UserId.Value,
                                        score = score,
                                        winnerId = winnerUserId
                                    
                                };
                                db.matchHistory.Add(history);
                            }
                        }

                        db.SaveChanges();
                        _logger.LogInfo($"Match stats saved for Game {lobby.GameCode}. Match ID: {match.matchId}");
                    }
                }, _logger, "OnGameEnded_BackgroundTask");
            });
        }

        public void FlipCard(int cardIndex)
        {
            ServerExceptionManager.SafeExecute(() =>
            {
                var sessionId = _callbackProvider.GetSessionId();
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
                        _logger.LogInfo($"Player {playerId} flipped card {cardIndex} in session {sessionId}.");
                    }
                }
            }, _logger, nameof(FlipCard));
        }

        public void VoteToKick(string targetUsername)
        {
            ServerExceptionManager.SafeExecute(() =>
            {
                var sessionId = _callbackProvider.GetSessionId();
                var lobby = _stateManager.GetLobbyBySession(sessionId);

                if (lobby == null)
                {
                    return;
                }

                var gameManager = _stateManager.GetGameManager(sessionId);
                var voterId = _stateManager.GetPlayerId(sessionId);

                if (gameManager != null && voterId != null)
                {
                    var targetClient = lobby.Clients.Values.FirstOrDefault(c => c.Name == targetUsername);

                    if (targetClient != null)
                    {
                        gameManager.HandleVoteKick(voterId, targetClient.Id);
                    }
                }
            }, _logger, nameof(VoteToKick));
        }

        private void HandleDisconnection(string sessionId)
        {
            var lobby = _stateManager.GetLobbyBySession(sessionId);
            var client = _stateManager.RemoveClient(sessionId, out var gameCode);

            if (client != null && lobby != null)
            {
                _notifier.NotifyLeave(lobby, client.Name);
                _logger.LogInfo($"Client {client.Name} disconnected from lobby {gameCode} (Session: {sessionId})");
            }
            else
            {
                _logger.LogWarn($"Client disconnected but lobby could not be resolved. Session: {sessionId}");
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

        private LobbyClient PrepareClient(string token, bool isGuest, string guestName)
        {
            var callback = _callbackProvider.GetCallback();

            string sessionId = _callbackProvider.GetSessionId() ?? Guid.NewGuid().ToString();

            if (_stateManager.IsInLobby(sessionId))
            {
                _logger.LogWarn($"Session {sessionId} attempted to join multiple lobbies.");
                return null;
            }

            string playerName = ResolvePlayerIdentity(token, isGuest, guestName, out int? userId);

            if (string.IsNullOrEmpty(playerName))
            {
                _logger.LogWarn($"Failed to resolve player name for session {sessionId}.");
                return null;
            }

            _logger.LogInfo($"Preparing LobbyClient for player {playerName} (Session: {sessionId})");

            return new LobbyClient
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = playerName,
                IsGuest = isGuest,
                Callback = callback,
                JoinedAt = DateTime.UtcNow,
                SessionId = sessionId,
                UserId = userId
            };
        }

        private string ResolvePlayerIdentity(string token, bool isGuest, string guestName, out int? userId)
        {
            userId = null;

            if (isGuest)
            {
                _logger.LogInfo("Resolving guest player name.");
                return _validator.IsValidGuestName(guestName) ? guestName.Trim() : null;
            }

            userId = _sessionManager.GetUserIdFromToken(token);

            _logger.LogInfo(userId.HasValue
                ? $"Resolved player name for user ID {userId.Value}."
                : "Failed to resolve player name from token.");

            return userId.HasValue ? _securityService.GetUsernameById(userId.Value) : null;
        }

        public bool SendInvitationEmail(string targetEmail, string subject, string body)
        {
            return ServerExceptionManager.SafeExecute(() =>
            {
                if (string.IsNullOrWhiteSpace(targetEmail) ||
                    string.IsNullOrWhiteSpace(subject) ||
                    string.IsNullOrWhiteSpace(body))
                {
                    _logger.LogWarn("Invalid email parameters for invitation email.");
                    return false;
                }

                MailMessage message = new MailMessage();

                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["EmailSender"];
                message.From = new MailAddress(senderEmail);

                message.To.Add(targetEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;
                IMailSender mailSender = new MailWrapper();
                mailSender.Send(message);

                _logger.LogInfo($"Email sent to {targetEmail}. Subject: {subject}");
                return true;
            }, _logger, nameof(SendInvitationEmail));
        }

        public List<LobbySummaryDTO> GetPublicLobbies()
        {
            return ServerExceptionManager.SafeExecute(() =>
            {
                var allLobbies = _stateManager.GetAllLobbies();

                var publicLobbies = allLobbies
                    .Where(lobby => lobby.IsPublic && !_stateManager.IsGameStarted(lobby.GameCode))
                    .Select(lobby => new LobbySummaryDTO
                    {
                        GameCode = lobby.GameCode,
                        CurrentPlayers = lobby.Clients.Count,
                        IsFull = lobby.Clients.Count >= 4
                    })
                    .ToList();

                return publicLobbies;
            }, _logger, nameof(GetPublicLobbies));
        }
    }
}