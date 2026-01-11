using Server.GameService;
using System.ServiceModel;

namespace Server.LobbyService
{
    [ServiceContract(CallbackContract = typeof(IGameLobbyCallback))]
    public interface IGameLobbyService
    {
        [OperationContract]
        bool JoinLobby(string token, string gameCode, bool isGuest, string guestName = null);
        [OperationContract]
        bool CreateLobby(string token, string gameCode);
        [OperationContract]
        void LeaveLobby();
        [OperationContract]
        void SendChatMessage(string message);
        [OperationContract]
        void StartGame(GameSettings settings);
        [OperationContract]
        void FlipCard(int cardIndex);
        [OperationContract]
        void VoteToKick(string targetUsername);
        [OperationContract]
        bool SendInvitationEmail(string targetEmail, string subject, string body);
    }
}
