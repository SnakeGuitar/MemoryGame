using Client.GameLobbyServiceReference;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Core
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class GameServiceManager : IGameLobbyServiceCallback
    {
        #region Singleton & Properties

        private static GameServiceManager _instance;
        public static GameServiceManager Instance => _instance ?? (_instance = new GameServiceManager());

        public GameLobbyServiceClient Client { get; private set; }


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
                    try { Client.Close(); } catch { Client.Abort(); }
                }

                InstanceContext context = new InstanceContext(this);
                Client = new GameLobbyServiceClient(context);
                Client.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GameServiceManager] Init Failed: {ex.Message}");
            }
        }

        private void EnsureConnection()
        {
            if (Client == null ||
                Client.State == CommunicationState.Closed ||
                Client.State == CommunicationState.Faulted)
            {
                InitializeClient();
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

        #region Lobby Wrappers

        public async Task<bool> CreateLobbyAsync(string token, string matchCode, bool isPublic)
        {
            EnsureConnection();
            return await Client.CreateLobbyAsync(token, matchCode, isPublic);
        }

        public async Task<LobbySummaryDTO[]> GetPublicLobbiesAsync()
        {
            EnsureConnection();
            return await Client.GetPublicLobbiesAsync();
        }

        public async Task<bool> JoinLobbyAsync(string token, string matchCode, bool isGuest, string guestUsername)
        {
            EnsureConnection();
            return await Client.JoinLobbyAsync(token, matchCode, isGuest, guestUsername);
        }

        public async Task LeaveLobbyAsync()
        {
            if (Client != null && Client.State == CommunicationState.Opened)
            {
                try
                {
                    await Client.LeaveLobbyAsync();
                }
                catch
                {
                }
            }
        }

        public void StartGameSafe(GameSettings settings)
        {
            EnsureConnection();
            Client.StartGame(settings);
        }

        public async Task<bool> SendInvitationEmailAsync(string targetEmail, string subject, string body)
        {
            EnsureConnection();
            return await Client.SendInvitationEmailAsync(targetEmail, subject, body);
        }

        #endregion

        #region Game Action Wrappers

        public async Task FlipCardAsync(int cardIndex)
        {
            EnsureConnection();
            await Client.FlipCardAsync(cardIndex);
        }

        public async Task SendChatMessageAsync(string message)
        {
            EnsureConnection();
            await Client.SendChatMessageAsync(message);
        }

        public async Task VoteToKickAsync(string playerToKick)
        {
            EnsureConnection();
            await Client.VoteToKickAsync(playerToKick);
        }

        #endregion
    }
}