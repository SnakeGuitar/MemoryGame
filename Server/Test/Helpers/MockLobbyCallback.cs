using Server.GameService;
using Server.LobbyService;
using System.Collections.Generic;

namespace Test.Helpers
{
    public class MockLobbyCallback : IGameLobbyCallback
    {
        public void CardFlipped(int cardIndex, int cardIndex2)
        {
        }

        public void GameFinished(string winnerName)
        {
        }

        public void GameStarted(List<CardInfo> gameBoard)
        {
        }

        public void HideCards(int cardIndex, int cardIndex2)
        {
        }

        public void PlayerJoined(string playerName, bool isGuest)
        {
        }

        public void PlayerLeft(string playerName)
        {
        }

        public void ReceiveChatMessage(string senderName, string message, bool isNotification)
        {
        }

        public void SetCardsAsMatched(int cardIndex1, int cardIndex2)
        {
        }

        public void ShowCard(int cardIndex, string imageIdentifier)
        {
        }

        public void UpdatePlayerList(LobbyPlayerInfo[] players)
        {
        }

        public void UpdateScore(string playerName, int newScore)
        {
        }

        public void UpdateTurn(string playerName, int turnTimeInSeconds)
        {
        }
    }
}
