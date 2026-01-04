using Server.GameService;
using Server.LobbyService;
using System.Collections.Generic;

namespace Test.Helpers
{
    /// <summary>
    /// Dummy IGameLobbyCallback for testing.
    /// </summary>
    public class MockLobbyCallback : IGameLobbyCallback
    {
        public bool MessageReceived { get; private set; }
        public string LastMessage { get; private set; }

        public void ReceiveChatMessage(string senderName, string message, bool isNotification)
        {
            MessageReceived = true;
            LastMessage = message;
        }

        public void PlayerJoined(string playerName, bool isGuest) { }

        public void PlayerLeft(string playerName) { }

        public void UpdatePlayerList(LobbyPlayerInfo[] players) { }

        public void GameStarted(List<CardInfo> gameBoard) { }

        public void UpdateTurn(string playerName, int turnTimeInSeconds) { }

        public void ShowCard(int cardIndex, string imageIdentifier) { }

        public void HideCards(int cardIndex1, int cardIndex2) { }

        public void CardFlipped(int cardIndex, int cardIndex2) { }

        public void SetCardsAsMatched(int cardIndex1, int cardIndex2) { }

        public void UpdateScore(string playerName, int newScore) { }

        public void GameFinished(string winnerName) { }
    }
}