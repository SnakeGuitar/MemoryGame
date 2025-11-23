using Server.GameService;
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
        void PlayerJoined(string playerName, bool isGuest);
        [OperationContract(IsOneWay = true)]
        void PlayerLeft(string playerName);
        [OperationContract(IsOneWay = true)]
        void UpdatePlayerList(LobbyPlayerInfo[] players);


        [OperationContract(IsOneWay = true)]
        void GameStarted(List<CardInfo> gameBoard);
        [OperationContract(IsOneWay = true)]
        void UpdateTurn(string playerName, int turnTimeInSeconds);
        [OperationContract(IsOneWay = true)]
        void ShowCard(int cardIndex, string imageIdentifier);
        [OperationContract(IsOneWay = true)]
        void HideCards(int cardIndex, int cardIndex2);
        [OperationContract(IsOneWay = true)]
        void CardFlipped(int cardIndex, int cardIndex2);
        [OperationContract(IsOneWay = true)]
        void SetCardsAsMatched(int cardIndex1, int cardIndex2);
        [OperationContract(IsOneWay = true)]
        void UpdateScore(string playerName, int newScore);
        [OperationContract(IsOneWay = true)]
        void GameFinished(string winnerName);
    }
}
