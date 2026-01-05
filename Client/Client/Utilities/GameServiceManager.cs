using Client.GameLobbyServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Utilities
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class GameServiceManager : IGameLobbyServiceCallback
    {
        private static GameServiceManager _instance;
        public static GameServiceManager Instance => _instance ?? (_instance = new GameServiceManager());

        public GameLobbyServiceClient Client { get; private set; }

        public event Action<string, string, bool> ChatMessageReceived;
        public event Action<string, bool> PlayerJoined;
        public event Action<string> PlayerLeft;
        public event Action<LobbyPlayerInfo[]> PlayerListUpdated;
        public event Action<List<CardInfo>> GameStarted;

        public event Action<string, int> TurnUpdated;
        public event Action<int, string> CardShown;
        public event Action<int, int> CardsHidden;
        public event Action<int, int> CardFlipped;
        public event Action<int, int> CardsMatched;
        public event Action<string, int> ScoreUpdated;
        public event Action<string> GameFinished;

        private GameServiceManager()
        {
            InstanceContext context = new InstanceContext(this);
            Client = new GameLobbyServiceClient(context);
        }

        public void ReceiveChatMessage(string senderName, string message, bool isNotification)
        {
            ChatMessageReceived?.Invoke(senderName, message, isNotification);
        }

        void IGameLobbyServiceCallback.PlayerJoined(string playerName, bool isGuest)
        {
            PlayerJoined?.Invoke(playerName, isGuest);
        }

        void IGameLobbyServiceCallback.PlayerLeft(string playerName)
        {
            PlayerLeft?.Invoke(playerName);
        }

        public void UpdatePlayerList(LobbyPlayerInfo[] players)
        {
            PlayerListUpdated?.Invoke(players);
        }

        void IGameLobbyServiceCallback.GameStarted(CardInfo[] gameBoard)
        {
            GameStarted?.Invoke(gameBoard.ToList());
        }

        public void UpdateTurn(string playerName, int turnTimeInSeconds)
        {
            TurnUpdated?.Invoke(playerName, turnTimeInSeconds);
        }

        public void ShowCard(int cardIndex, string imageIdentifier)
        {
            CardShown?.Invoke(cardIndex, imageIdentifier);
        }

        public void HideCards(int cardIndex1, int cardIndex2)
        {
            CardsHidden?.Invoke(cardIndex1, cardIndex2);
        }

        void IGameLobbyServiceCallback.CardFlipped(int cardIndex, int cardIndex2)
        {
            CardFlipped?.Invoke(cardIndex, cardIndex2);
        }

        public void SetCardsAsMatched(int cardIndex1, int cardIndex2)
        {
            CardsMatched?.Invoke(cardIndex1, cardIndex2);
        }

        public void UpdateScore(string playerName, int newScore)
        {
            ScoreUpdated?.Invoke(playerName, newScore);
        }

        void IGameLobbyServiceCallback.GameFinished(string winnerName)
        {
            GameFinished?.Invoke(winnerName);
        }
    }
}
