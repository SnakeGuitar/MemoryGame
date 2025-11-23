using Server.GameService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.LobbyService.Core
{
    public class LobbyStateManager
    {
        private readonly ConcurrentDictionary<string, Lobby> _lobbies = new ConcurrentDictionary<string, Lobby>();
        private readonly ConcurrentDictionary<string, GameManager> _games = new ConcurrentDictionary<string, GameManager>();
        private readonly ConcurrentDictionary<string, string> _sessionToLobbyCode = new ConcurrentDictionary<string, string>();

        public bool IsInLobby(string sessionId)
        {
            return _sessionToLobbyCode.ContainsKey(sessionId);
        }

        public bool IsGameStarted(string gameCode)
        {
            return _games.ContainsKey(gameCode);
        }

        public bool TryJoinLobby(string gameCode, LobbyClient client, out Lobby lobby)
        {
            lobby = _lobbies.GetOrAdd(gameCode, code => new Lobby
            {
                GameCode = code,
                CreatedAt = DateTime.UtcNow
            });

            lock (lobby.LockObject)
            {
                if (lobby.Clients.Count >= 4) return false;

                if (lobby.Clients.TryAdd(client.Id, client))
                {
                    _sessionToLobbyCode.TryAdd(client.SessionId, gameCode);
                    return true;
                }
            }
            return false;
        }

        public LobbyClient RemoveClient(string sessionId, out string gameCode)
        {
            gameCode = null;
            if (!_sessionToLobbyCode.TryRemove(sessionId, out gameCode)) return null;

            if (_lobbies.TryGetValue(gameCode, out var lobby))
            {
                var client = lobby.Clients.Values.FirstOrDefault(c => c.SessionId == sessionId);
                if (client != null)
                {
                    lobby.Clients.TryRemove(client.Id, out _);

                    if (lobby.Clients.IsEmpty)
                    {
                        lock (lobby.LockObject)
                        {
                            if (lobby.Clients.IsEmpty)
                            {
                                _lobbies.TryRemove(gameCode, out _);
                                _games.TryRemove(gameCode, out _);
                            }
                        }
                    }
                    return client;
                }
            }
            return null;
        }

        public Lobby GetLobbyBySession(string sessionId)
        {
            if (_sessionToLobbyCode.TryGetValue(sessionId, out var gameCode))
            {
                if (_lobbies.TryGetValue(gameCode, out var lobby)) return lobby;
            }
            return null;
        }

        public bool TryStartGame(string sessionId, GameSettings settings, out string gameCode)
        {
            var lobby = GetLobbyBySession(sessionId);
            if (lobby == null)
            {
                gameCode = null;
                return false;
            }

            gameCode = lobby.GameCode;
            if (_games.ContainsKey(gameCode)) return false;

            var players = lobby.Clients.Values.ToList();
            var gameManager = new GameManager(players, settings);

            return _games.TryAdd(gameCode, gameManager);
        }

        public GameManager GetGameManager(string sessionId)
        {
            if (_sessionToLobbyCode.TryGetValue(sessionId, out var gameCode))
            {
                if (_games.TryGetValue(gameCode, out var manager)) return manager;
            }
            return null;
        }

        public string GetPlayerId(string sessionId)
        {
            var lobby = GetLobbyBySession(sessionId);
            return lobby?.Clients.Values.FirstOrDefault(c => c.SessionId == sessionId)?.Id;
        }
    }
}
