using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.LobbyService
{
    [ServiceContract]
    public interface IGameLobbyCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveChatMessage(string senderName, string message, bool isNotification);
        [OperationContract(IsOneWay = true)]
        void PlayerJoined(string playerNmae, bool  isGuest);
        [OperationContract(IsOneWay = true)]
        void playerLeft(string playerNmae);
        [OperationContract(IsOneWay = true)]
        void UpdatePlayerList(LobbyPlayerInfo[] players);
    }
}
