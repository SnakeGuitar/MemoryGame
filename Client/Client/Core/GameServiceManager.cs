using Client.GameLobbyServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Core
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class GameServiceManager : IGameLobbyServiceCallback
    {
        #region Singleton & Properties

        private static GameServiceManager _instance;
        public static GameServiceManager Instance => _instance ?? (_instance = new GameServiceManager());

        public GameLobbyServiceClient Client { get; private set; }

        private ServerConnectionMonitor _connectionMonitor;

        #endregion

        #region Events

        public event Action ServerConnectionLost;

        public event Action<string, string, bool> ChatMessageReceived;
        public event Action<string, bool> PlayerJoined;
        public event Action<string> PlayerLeft;
        public event Action<LobbyPlayerInfo[]> PlayerListUpdated;
        public event Action<List<CardInfo>> GameStarted;

        public event Action<string, int> TurnUpdated;
        public event Action<int, string> CardShown;
        public event Action<int, int> CardsHidden;
        public event Action<int, int> CardsMatched;
        public event Action<string, int> ScoreUpdated;
        public event Action<string> GameFinished;

        #endregion

        private GameServiceManager()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            try
            {
                if (Client != null)
                {
                    try
                    {
                        Client.Close();
                    }
                    catch
                    {
                        Client.Abort();
                    }
                }
                _connectionMonitor?.Stop();

                InstanceContext context = new InstanceContext(this);
                Client = new GameLobbyServiceClient(context);

                Client.Open();

                if (Client.InnerChannel != null)
                {
                    Client.InnerChannel.Faulted += (s, e) => NotifyDisconnect();
                }

                _connectionMonitor = new ServerConnectionMonitor(async () =>
                {
                    try
                    {
                        if (Client == null ||
                            Client.State == CommunicationState.Closed ||
                            Client.State == CommunicationState.Faulted)
                        {
                            return false;
                        }

                        await Client.PingAsync();
                        
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                });

                _connectionMonitor.ConnectionLost += NotifyDisconnect;
                _connectionMonitor.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GameServiceManager] Init Failed: {ex.Message}");
                NotifyDisconnect();
            }
        }

        private void NotifyDisconnect()
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                ServerConnectionLost?.Invoke();
            });
        }

        /// <summary>
        /// Wrapper method to make the call semantic and safer.
        /// </summary>
        public async Task FlipCardAsync(int cardIndex)
        {
            if (Client != null && Client.State == CommunicationState.Opened)
            {
                await Client.FlipCardAsync(cardIndex);
            }
        }

        #region IGameLobbyServiceCallback Implementation

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

        #endregion
    }
}