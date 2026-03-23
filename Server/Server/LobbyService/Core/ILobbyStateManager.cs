using System.Collections.Generic;

namespace Server.LobbyService.Core
{
    public interface ILobbyStateManager
    {
        bool LobbyExists(string gameCode);
        bool IsInLobby(string sessionId);
        bool IsGameStarted(string gameCode);
        bool TryJoinLobby(string gameCode, LobbyClient client, out Lobby lobby);
        bool TryCreateLobby(string gameCode, LobbyClient client, out Lobby lobby);
        LobbyClient RemoveClient(string sessionId, out string gameCode);
        Lobby GetLobbyBySession(string sessionId);
        bool TryStartGame(string sessionId, GameSettings settings, out string gameCode);
        Server.GameService.GameManager GetGameManager(string sessionId);
        string GetPlayerId(string sessionId);
        IEnumerable<Lobby> GetAllLobbies();
    }
}
