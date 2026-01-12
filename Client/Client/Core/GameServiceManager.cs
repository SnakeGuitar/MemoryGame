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
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.CreateLobbyAsync(token, matchCode, isPublic);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[CreateLobby] Error: {ex.Message}");
                    return false;
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return false;
        }

        public async Task<LobbySummaryDTO[]> GetPublicLobbiesAsync()
        {
            if (EnsureConnection())
            {
                try
                {
                    return await Client.GetPublicLobbiesAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[GetPublicLobbies] Error: {ex.Message}");
                    return Array.Empty<LobbySummaryDTO>();
                }
            }
            return Array.Empty<LobbySummaryDTO>();
        }

        public async Task<bool> JoinLobbyAsync(string token, string matchCode, bool isGuest, string guestUsername)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.JoinLobbyAsync(token, matchCode, isGuest, guestUsername);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[JoinLobby] Error: {ex.Message}");
                    return false;
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return false;
        }

        public async Task LeaveLobbyAsync()
        {
            if (Client != null && Client.State == CommunicationState.Opened)
            {
                _connectionMonitor?.Stop();
                try
                {
                    await Client.LeaveLobbyAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[LeaveLobby] Ignored Error: {ex.Message}");
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
        }

        public void StartGameSafe(GameSettings settings)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    Client.StartGame(settings);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[StartGame] Error: {ex.Message}");
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
        }

        public async Task<bool> SendInvitationEmailAsync(string targetEmail, string subject, string body)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.SendInvitationEmailAsync(targetEmail, subject, body);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Email Error] {ex.Message}");
                    return false;
                }
                finally
                {
                    _connectionMonitor?.Start();
                }
            }
            return false;
        }

        #endregion

        #region Game Action Wrappers

        public async Task FlipCardAsync(int cardIndex)
        {
            if (EnsureConnection())
            {
                try
                {
                    await Client.FlipCardAsync(cardIndex);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[FlipCard] Error: {ex.Message}");
                }
            }
        }

        public async Task SendChatMessageAsync(string message)
        {
            if (EnsureConnection())
            {
                try
                {
                    await Client.SendChatMessageAsync(message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Chat] Error: {ex.Message}");
                }
            }
        }

        public async Task VoteToKickAsync(string playerToKick)
        {
            if (EnsureConnection())
            {
                try
                {
                    await Client.VoteToKickAsync(playerToKick);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[VoteKick] Error: {ex.Message}");
                }
            }
        }

        #endregion

        private bool EnsureConnection()
        {
            if (Client == null ||
                Client.State == CommunicationState.Closed ||
                Client.State == CommunicationState.Faulted)
            {
                InitializeClient();
            }

            if (Client == null)
            {
                return false;
            }
            return Client.State == CommunicationState.Opened;
        }
    }
}