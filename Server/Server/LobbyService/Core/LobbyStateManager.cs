using Server.GameService;
using Server.Shared;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Server.LobbyService.Core
{
    public class LobbyStateManager
    {
        private readonly ConcurrentDictionary<string, Lobby> _lobbies = new ConcurrentDictionary<string, Lobby>();
        private readonly ConcurrentDictionary<string, GameManager> _games = new ConcurrentDictionary<string, GameManager>();
        private readonly ConcurrentDictionary<string, string> _sessionToLobbyCode = new ConcurrentDictionary<string, string>();

        private readonly ILoggerManager _logger;

        public LobbyStateManager(ILoggerManager logger)
        {
            _logger = logger;
        }

        public LobbyStateManager() : this(new Logger(typeof(LobbyStateManager)))
        {
        }

        public bool LobbyExists(string gameCode)
        {
            return _lobbies.ContainsKey(gameCode);
        }

        public bool IsInLobby(string sessionId)
        {
            _logger.LogInfo($"Checking if session {sessionId} is in a lobby.");
            return _sessionToLobbyCode.ContainsKey(sessionId);
        }

        public bool IsGameStarted(string gameCode)
        {
            _logger.LogInfo($"Checking if game with code {gameCode} has started.");
            return _games.ContainsKey(gameCode);
        }

        public bool TryJoinLobby(string gameCode, LobbyClient client, out Lobby lobby)
        {
            if (!_lobbies.TryGetValue(gameCode, out lobby))
            {
                _logger.LogWarn($"Client {client.Id} tried to join non-existent lobby {gameCode}.");
                return false;
            }

            lock (lobby.LockObject)
            {
                if (lobby.Clients.Count >= 4)
                {
                    _logger.LogWarn($"Lobby {gameCode} is full.");
                    return false;
                }

                if (lobby.Clients.TryAdd(client.Id, client))
                {
                    _sessionToLobbyCode.TryAdd(client.SessionId, gameCode);
                    return true;
                }
            }
            return false;
        }

        public bool TryCreateLobby(string gameCode, LobbyClient client, out Lobby lobby)
        {
            lobby = new Lobby
            {
                GameCode = gameCode,
                CreatedAt = DateTime.UtcNow
            };

            if (_lobbies.TryAdd(gameCode, lobby))
            {
                if (lobby.Clients.TryAdd(client.Id, client))
                {
                    _sessionToLobbyCode.TryAdd(client.SessionId, gameCode);
                    _logger.LogInfo($"Lobby {gameCode} created and client {client.Id} added.");
                    return true;
                }
                else
                {
                    _lobbies.TryRemove(gameCode, out _);
                    _logger.LogError($"Failed to add client {client.Id} to newly created lobby {gameCode}. Lobby removed.");
                }
            }
                _logger.LogWarn($"Lobby {gameCode} already exists. Cannot create new lobby.");
                return false;
        }

        public LobbyClient RemoveClient(string sessionId, out string gameCode)
        {
            gameCode = null;
            if (!_sessionToLobbyCode.TryRemove(sessionId, out gameCode))
            {
                _logger.LogWarn($"Session {sessionId} not found in any lobby.");
                return null;
            }

            if (_lobbies.TryGetValue(gameCode, out var lobby))
            {
                var client = lobby.Clients.Values.FirstOrDefault(c => c.SessionId == sessionId);
                if (client != null)
                {
                    lobby.Clients.TryRemove(client.Id, out _);
                    _logger.LogInfo($"Client {client.Id} removed from lobby {gameCode}.");

                    if (lobby.Clients.IsEmpty)
                    {
                        lock (lobby.LockObject)
                        {
                            if (lobby.Clients.IsEmpty)
                            {
                                _lobbies.TryRemove(gameCode, out _);
                                bool gameRemoved = _games.TryRemove(gameCode, out _);

                                _logger.LogInfo($"Lobby {gameCode} removed. Game removed: {gameRemoved}");
                            }
                        }
                    }
                    _logger.LogInfo($"Returning removed client {client.Id} from lobby {gameCode}.");
                    return client;
                }
            }
            else
            {
                _logger.LogWarn($"Lobby {gameCode} not found when trying to remove client with session {sessionId}.");
            }
            _logger.LogWarn($"Client with session {sessionId} not found in lobby {gameCode}.");
            return null;
        }

        public Lobby GetLobbyBySession(string sessionId)
        {
            if (_sessionToLobbyCode.TryGetValue(sessionId, out var gameCode))
            {
                if (_lobbies.TryGetValue(gameCode, out var lobby))
                { 
                    _logger.LogInfo($"Lobby {gameCode} retrieved for session {sessionId}.");
                    return lobby; 
                }
            }
            _logger.LogWarn($"No lobby found for session {sessionId}.");
            return null;
        }

        public bool TryStartGame(string sessionId, GameSettings settings, out string gameCode)
        {
            var lobby = GetLobbyBySession(sessionId);
            if (lobby == null)
            {
                gameCode = null;
                _logger.LogWarn($"No lobby found for session {sessionId}. Cannot start game.");
                return false;
            }

            gameCode = lobby.GameCode;
            if (_games.ContainsKey(gameCode))
            {
                _logger.LogWarn($"Game already started for lobby {gameCode}.");
                return false;
            }

            var players = lobby.Clients.Values.ToList();
            var gameManager = new GameManager(players, settings, _logger);

            if (_games.TryAdd(gameCode, gameManager))
            {
                _logger.LogInfo($"Game started for lobby {gameCode} with {players.Count} players.");
                gameManager.StartGame();
                return true;
            }
            else
            {
                _logger.LogError($"Failed to start game for lobby {gameCode}.");
                return false;
            }
        }

        public GameManager GetGameManager(string sessionId)
        {
            if (_sessionToLobbyCode.TryGetValue(sessionId, out var gameCode))
            {
                if (_games.TryGetValue(gameCode, out var manager))
                {
                    _logger.LogInfo($"GameManager retrieved for session {sessionId} in game {gameCode}.");
                    return manager; 
                }
            }
            _logger.LogWarn($"No GameManager found for session {sessionId}.");
            return null;
        }

        public string GetPlayerId(string sessionId)
        {
            var lobby = GetLobbyBySession(sessionId);
            _logger.LogInfo($"Retrieving player ID for session {sessionId}.");
            return lobby?.Clients.Values.FirstOrDefault(c => c.SessionId == sessionId)?.Id;
        }
    }
}
