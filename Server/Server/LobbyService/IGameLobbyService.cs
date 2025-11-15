using System.ServiceModel;

namespace Server.LobbyService
{
    [ServiceContract(CallbackContract = typeof(LobbyService.IGameLobbyCallback))]
    public interface IGameLobbyService
    {
        [OperationContract]
        bool JoinLobby(string token, string gameCode, bool isGuest, string guestName = null);
        [OperationContract]
        void LeaveLobby();
        [OperationContract]
        void SendChatMessage(string message);
    }
}
