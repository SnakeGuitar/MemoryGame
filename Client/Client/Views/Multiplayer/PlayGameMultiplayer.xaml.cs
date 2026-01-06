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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.Views.Multiplayer
{
    /// <summary>
    /// Interaction logic for PlayGameMultiplayer.xaml.
    /// Manages the synchronized game session, turn logic, and UI updates for multiple players.
    /// </summary>
    public partial class PlayGameMultiplayer : Window
    {
        #region Private Fields & Constants

        /// <summary>
        /// Maximum number of messages to keep in the chat list to prevent memory issues.
        /// </summary>
        private const int MaxChatMessages = 50;

        /// <summary>
        /// Manages the core game logic (timer, matches) locally.
        /// </summary>
        private readonly GameManager _gameManager;

        /// <summary>
        /// List of players participating in the current match.
        /// </summary>
        private readonly List<LobbyPlayerInfo> _players;

        /// <summary>
        /// Tracks the username of the player whose turn is currently active.
        /// Used to direct timer and score updates to the correct UI slot.
        /// </summary>
        private string _currentTurnPlayer;

        private Border[] _playerBorders;
        private Label[] _playerNames;
        private Label[] _playerScores;
        private Label[] _playerTimes;

        #endregion

        #region Public Properties

        /// <summary>
        /// Collection of cards bound to the GameBoard UI.
        /// </summary>
        public ObservableCollection<Card> Cards { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the multiplayer game window.
        /// </summary>
        /// <param name="serverCards">The shuffled list of cards received from the server.</param>
        /// <param name="players">The list of players in the lobby.</param>
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

            _currentTurnPlayer = _players.FirstOrDefault()?.Name;
            HighlightActivePlayer(_currentTurnPlayer);

            StartGameSafe(serverCards);
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Maps XAML controls to arrays for easy index-based manipulation.
        /// </summary>
        private void InitializeUIArrays()
        {
            _playerBorders = new Border[] { BorderP1, BorderP2, BorderP3, BorderP4 };
            _playerNames = new Label[] { LabelP1Name, LabelP2Name, LabelP3Name, LabelP4Name };
            _playerScores = new Label[] { LabelP1Score, LabelP2Score, LabelP3Score, LabelP4Score };
            _playerTimes = new Label[] { LabelP1Time, LabelP2Time, LabelP3Time, LabelP4Time };
        }

        /// <summary>
        /// Configures the visibility and initial content of player panels based on the lobby list.
        /// </summary>
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

        /// <summary>
        /// Starts the game logic within a try-catch block to handle potential startup errors.
        /// </summary>
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

        /// <summary>
        /// Subscribes to GameManager and GameServiceManager events.
        /// </summary>
        private void ConfigureEvents()
        {
            _gameManager.TimerUpdated += OnTimerUpdated;
            _gameManager.ScoreUpdated += OnScoreUpdated;
            _gameManager.GameWon += OnGameWon;
            _gameManager.GameLost += OnGameLost;

            GameServiceManager.Instance.ChatMessageReceived += OnChatMessageReceived;
            GameServiceManager.Instance.PlayerLeft += OnPlayerLeft;
        }

        /// <summary>
        /// Unsubscribes from all events to prevent memory leaks and ghost updates.
        /// </summary>
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

        #region Game Logic Callbacks

        /// <summary>
        /// Called when the game timer ticks. Updates the UI timer for the current turn player.
        /// </summary>
        /// <param name="timeString">Formatted time string (e.g., "00:30").</param>
        private void OnTimerUpdated(string timeString)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                int activeIndex = _players.FindIndex(p => p.Name == _currentTurnPlayer);

                if (activeIndex >= 0 && activeIndex < _playerTimes.Length)
                {
                    if (_playerTimes[activeIndex] != null)
                    {
                        _playerTimes[activeIndex].Content = $"Time: {timeString}";
                    }
                }
            });
        }

        /// <summary>
        /// Called when a match is found. Updates the score for the current turn player.
        /// </summary>
        /// <param name="newScore">The accumulated score.</param>
        private void OnScoreUpdated(int newScore)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                int activeIndex = _players.FindIndex(p => p.Name == _currentTurnPlayer);

                if (activeIndex >= 0 && activeIndex < _playerScores.Length)
                {
                    int pairs = newScore / GameConstants.PointsPerMatch;
                    _playerScores[activeIndex].Content = $"Pairs: {pairs}";
                }
            });
        }

        /// <summary>
        /// Updates the local turn state when the server notifies a turn change.
        /// </summary>
        /// <param name="nextPlayerName">The username of the next player.</param>
        public void OnTurnUpdated(string nextPlayerName)
        {
            Dispatcher.Invoke(() =>
            {
                _currentTurnPlayer = nextPlayerName;

                foreach (var lbl in _playerTimes)
                {
                    if (lbl != null && lbl.Visibility == Visibility.Visible)
                        lbl.Content = "Time: --";
                }

                HighlightActivePlayer(nextPlayerName);
            });
        }

        /// <summary>
        /// Visually highlights the active player's name.
        /// </summary>
        private void HighlightActivePlayer(string playerName)
        {
            for (int i = 0; i < _playerNames.Length; i++)
            {
                if (i < _players.Count)
                {
                    if (_players[i].Name == playerName)
                    {
                        _playerNames[i].Foreground = Brushes.Gold;
                        _playerNames[i].FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        _playerNames[i].Foreground = Brushes.White;
                        _playerNames[i].FontWeight = FontWeights.Normal;
                    }
                }
            }
        }

        private void OnGameWon()
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;

                string winnerName = DetermineWinner();
                string statsInfo = $"{Lang.Global_Label_Score}: {GetMyCurrentScore()}";

                ShowMatchSummary(winnerName + " Wins!", statsInfo);
            });
        }

        private void OnGameLost()
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded) return;
                ShowMatchSummary(Lang.Singleplayer_Title_TimeOver, $"{Lang.Global_Label_Score}: {GetMyCurrentScore()}");
            });
        }

        #endregion

        #region Service Callbacks (Chat & Connection)

        /// <summary>
        /// Handles incoming chat messages from the server.
        /// </summary>
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

        /// <summary>
        /// Handles a player leaving the game session.
        /// </summary>
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

        #region UI Interactions

        /// <summary>
        /// Handles card click events.
        /// </summary>
        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTurnPlayer != UserSession.Username)
            {
                return;
            }

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

        /// <summary>
        /// Sends a chat message to the lobby.
        /// </summary>
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

        private string DetermineWinner()
        {
            return "Game Over";
        }

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

        /// <summary>
        /// Safely leaves the lobby and closes the window.
        /// </summary>
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
                System.Diagnostics.Debug.WriteLine($"[LeaveGame Warning]: {ex.Message}");
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

        /// <summary>
        /// Ensures cleanup when the window is closed by external means (Alt+F4).
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
            _gameManager.StopGame();

            try
            {
                GameServiceManager.Instance.Client.LeaveLobbyAsync();
            }
            catch { }

            base.OnClosed(e);
        }

        #endregion
    }
}