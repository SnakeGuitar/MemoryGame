using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Models;
using Client.Properties.Langs;
using Client.Utilities;
using Client.ViewModels;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.Views.Multiplayer
{
    /// <summary>
    /// Interaction logic for PlayGameMultiplayer.xaml.
    /// Implements synchronized gameplay, thread-safe UI updates, and robust resource cleanup.
    /// </summary>
    public partial class PlayGameMultiplayer : Window
    {
        #region Private Fields & Constants
        private const int MaxChatMessages = 100;

        private readonly GameManager _gameManager;
        private readonly List<LobbyPlayerInfo> _players;

        private Border[] _playerBorders;
        private Label[] _playerNames;
        private Label[] _playerScores;
        private Label[] _playerTimes;
        #endregion

        #region Properties
        public ObservableCollection<Card> Cards { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the multiplayer game session.
        /// </summary>
        /// <param name="serverCards">The deck configuration received from the server.</param>
        /// <param name="players">The list of participants.</param>
        public PlayGameMultiplayer(List<CardInfo> serverCards, List<LobbyPlayerInfo> players)
        {
            InitializeComponent();

            _players = players ?? new List<LobbyPlayerInfo>();
            Cards = new ObservableCollection<Card>();

            InitializeUIArrays();

            GameBoard.ItemsSource = Cards;
            this.DataContext = this;

            SetupPlayerUI();

            _gameManager = new GameManager(Cards);
            ConfigureEvents();

            StartGameSafe(serverCards);
        }

        #region Initialization

        private void InitializeUIArrays()
        {
            _playerBorders = new Border[] { BorderP1, BorderP2, BorderP3, BorderP4 };
            _playerNames = new Label[] { LabelP1Name, LabelP2Name, LabelP3Name, LabelP4Name };
            _playerScores = new Label[] { LabelP1Score, LabelP2Score, LabelP3Score, LabelP4Score };
            _playerTimes = new Label[] { LabelP1Time, LabelP2Time, LabelP3Time, LabelP4Time };
        }

        private void SetupPlayerUI()
        {
            for (int i = 0; i < _playerBorders.Length; i++)
            {
                if (i < _players.Count)
                {
                    _playerBorders[i].Visibility = Visibility.Visible;
                    _playerNames[i].Content = _players[i].Name;
                    _playerScores[i].Content = "Pairs: 0";
                    _playerTimes[i].Content = "Time: --";

                    if (_players[i].Name == UserSession.Username)
                    {
                        _playerNames[i].Foreground = Brushes.Gold;
                        _playerNames[i].FontWeight = FontWeights.Bold;
                    }
                }
                else
                {
                    _playerBorders[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void StartGameSafe(List<CardInfo> serverCards)
        {
            try
            {
                var config = new GameConfiguration
                {
                    NumberOfCards = serverCards.Count,
                    TimeLimitSeconds = GameConstants.DefaultTurnTimeSeconds
                };

                _gameManager.StartMultiplayerGame(config, serverCards);
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => this.Close());
            }
        }

        #endregion

        #region Event Management

        private void ConfigureEvents()
        {
            _gameManager.TimerUpdated += OnTimerUpdated;
            _gameManager.ScoreUpdated += OnScoreUpdated;
            _gameManager.GameWon += OnGameWon;
            _gameManager.GameLost += OnGameLost;

            GameServiceManager.Instance.ChatMessageReceived += OnChatMessageReceived;
            GameServiceManager.Instance.PlayerLeft += OnPlayerLeft;
        }

        private void UnsubscribeEvents()
        {
            _gameManager.TimerUpdated -= OnTimerUpdated;
            _gameManager.ScoreUpdated -= OnScoreUpdated;
            _gameManager.GameWon -= OnGameWon;
            _gameManager.GameLost -= OnGameLost;

            GameServiceManager.Instance.ChatMessageReceived -= OnChatMessageReceived;
            GameServiceManager.Instance.PlayerLeft -= OnPlayerLeft;
        }

        #endregion

        #region Game Callbacks (Thread-Safe)

        private void OnTimerUpdated(string timeString)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                if (_playerTimes[0] != null)
                {
                    _playerTimes[0].Content = $"Time: {timeString}";
                }
            });
        }

        private void OnScoreUpdated(int newScore)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                int myIndex = _players.FindIndex(p => p.Name == UserSession.Username);
                if (myIndex >= 0 && myIndex < _playerScores.Length)
                {
                    int pairs = newScore / GameConstants.PointsPerMatch;
                    _playerScores[myIndex].Content = $"Pairs: {pairs}";
                }
            });
        }

        private void OnGameWon()
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                string winnerName = UserSession.Username ?? "Player";
                string statsInfo = $"{Lang.Global_Label_Score}: {GetMyCurrentScore()}";
                ShowMatchSummary(winnerName, statsInfo);
            });
        }

        private void OnGameLost()
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                string title = Lang.Singleplayer_Title_TimeOver;
                string statsInfo = $"{Lang.Global_Label_Score}: {GetMyCurrentScore()}";
                ShowMatchSummary(title, statsInfo);
            });
        }

        #endregion

        #region Service Callbacks (Chat & Players)

        private void OnChatMessageReceived(string sender, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                string formattedMsg = isNotification ? $"--- {message} ---" : $"{sender}: {message}";
                ChatListBox.Items.Add(formattedMsg);

                if (ChatListBox.Items.Count > MaxChatMessages)
                {
                    ChatListBox.Items.RemoveAt(0);
                }

                if (ChatListBox.Items.Count > 0)
                {
                    ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
                }
            });
        }

        private void OnPlayerLeft(string playerName)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                ChatListBox.Items.Add($"--- {playerName} left the game ---");

                int index = _players.FindIndex(p => p.Name == playerName);
                if (index >= 0 && index < _playerBorders.Length)
                {
                    _playerBorders[index].Opacity = 0.5;
                    _playerNames[index].Content += " (Left)";
                }
            });
        }

        #endregion

        #region User Interaction

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card clickedCard)
            {
                try
                {
                    await _gameManager.HandleCardClick(clickedCard);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Handle(ex, this, null);
                }
            }
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ChatTextBox.Text))
            {
                string msg = ChatTextBox.Text;
                ChatTextBox.Text = string.Empty;

                try
                {
                    await GameServiceManager.Instance.Client.SendChatMessageAsync(msg);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Handle(ex, this, null);
                }
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.StopGame();
            var settingsWindow = new Settings();
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }

        private async void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            await LeaveGameSafe();
        }

        #endregion

        #region Helpers & Cleanup

        private string GetMyCurrentScore()
        {
            int myIndex = _players.FindIndex(p => p.Name == UserSession.Username);
            if (myIndex >= 0 && myIndex < _playerScores.Length)
            {
                return _playerScores[myIndex].Content.ToString();
            }
            return "0";
        }

        private void ShowMatchSummary(string title, string stats)
        {
            var summaryWindow = new MatchSummary(title, stats);
            summaryWindow.Owner = this;
            summaryWindow.ShowDialog();

            _ = LeaveGameSafe();
        }

        private async Task LeaveGameSafe()
        {
            UnsubscribeEvents();
            _gameManager.StopGame();

            try
            {
                await GameServiceManager.Instance.Client.LeaveLobbyAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[LeaveGame Warning]: {ex.Message}");
            }
            finally
            {
                if (this.Owner != null)
                {
                    this.Owner.Show();
                }
                this.Close();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
            _gameManager.StopGame();

            try { GameServiceManager.Instance.Client.LeaveLobbyAsync(); } catch { }

            base.OnClosed(e);
        }

        #endregion
    }
}