using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Models;
using Client.Properties.Langs;
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
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Multiplayer
{
    public partial class PlayGameMultiplayer : Window
    {
        #region Private Fields & Constants

        private const int MAX_CHAT_MESSAGES = 50;

        private readonly GameManager _gameManager;
        private readonly List<LobbyPlayerInfo> _players;
        private string _currentTurnPlayer;

        private Border[] _playerBorders;
        private Label[] _playerNames;
        private Label[] _playerScores;
        private Label[] _playerTimes;

        #endregion

        public ObservableCollection<Card> Cards { get; set; }

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

            if (_players.Count > 0)
            {
                _currentTurnPlayer = _players[0].Name;
                HighlightActivePlayer(_currentTurnPlayer);
                int activeIndex = _players.FindIndex(p => p.Name == _currentTurnPlayer);
                if (activeIndex >= 0 && activeIndex < _playerTimes.Length)
                {
                    _playerTimes[activeIndex].Content = $"{Lang.PlayGameMultiplayer_Label_TimeTurn} {GameConstants.DefaultTurnTimeSeconds}";
                }
            }

            StartGameSafe(serverCards);
        }

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

                    _playerBorders[i].Tag = _players[i].Name; // Important for context menu

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

        #region Event Management

        private void ConfigureEvents()
        {
            _gameManager.TimerUpdated += OnLocalTimerTick;

            var service = GameServiceManager.Instance;
            service.TurnUpdated += OnServerTurnUpdated;
            service.CardShown += OnServerCardShown;
            service.CardsHidden += OnServerCardsHidden;
            service.CardsMatched += OnServerCardsMatched;
            service.ScoreUpdated += OnServerScoreUpdated;
            service.GameFinished += OnServerGameFinished;
            service.ChatMessageReceived += OnChatMessageReceived;
            service.PlayerLeft += OnPlayerLeft;
        }

        private void UnsubscribeEvents()
        {
            _gameManager.TimerUpdated -= OnLocalTimerTick;

            var service = GameServiceManager.Instance;
            service.TurnUpdated -= OnServerTurnUpdated;
            service.CardShown -= OnServerCardShown;
            service.CardsHidden -= OnServerCardsHidden;
            service.CardsMatched -= OnServerCardsMatched;
            service.ScoreUpdated -= OnServerScoreUpdated;
            service.GameFinished -= OnServerGameFinished;
            service.ChatMessageReceived -= OnChatMessageReceived;
            service.PlayerLeft -= OnPlayerLeft;
        }

        #endregion

        #region Server Callbacks Implementation

        private void OnServerTurnUpdated(string nextPlayerName, int seconds)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                _currentTurnPlayer = nextPlayerName;

                foreach (var label in _playerTimes)
                {
                    if (label != null && label.Visibility == Visibility.Visible)
                    {
                        label.Content = $"{Lang.PlayGameMultiplayer_Label_TimeTurn} --";
                    }
                }

                HighlightActivePlayer(nextPlayerName);

                _gameManager.UpdateTurnDuration(seconds);
                _gameManager.ResetTurnTimer();
            });
        }

        private void OnServerCardShown(int cardIndex, string imageId)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                var card = Cards.FirstOrDefault(c => c.Id == cardIndex);
                if (card != null)
                {
                    card.IsFlipped = true;
                }
            });
        }

        private void OnServerCardsHidden(int idx1, int idx2)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                var c1 = Cards.FirstOrDefault(c => c.Id == idx1);
                var c2 = Cards.FirstOrDefault(c => c.Id == idx2);

                if (c1 != null)
                {
                    c1.IsFlipped = false;
                }
                if (c2 != null)
                {
                    c2.IsFlipped = false;
                }
            });
        }

        private void OnServerCardsMatched(int idx1, int idx2)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                var c1 = Cards.FirstOrDefault(c => c.Id == idx1);
                var c2 = Cards.FirstOrDefault(c => c.Id == idx2);

                if (c1 != null)
                {
                    c1.IsMatched = true;
                    c1.IsFlipped = true;
                }
                if (c2 != null)
                {
                    c2.IsMatched = true;
                    c2.IsFlipped = true;
                }
            });
        }

        private void OnServerScoreUpdated(string playerName, int score)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                int index = _players.FindIndex(p => p.Name == playerName);
                if (index >= 0 && index < _playerScores.Length)
                {
                    _playerScores[index].Content = $"{Lang.PlayGameMultiplayer_Label_FoundPairs} {score}";
                }
            });
        }

        private void OnServerGameFinished(string winnerName)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                string myScoreText = GetMyCurrentScore();
                ShowMatchSummary(winnerName, myScoreText);
            });
        }

        #endregion

        #region Local UI Logic

        private void OnLocalTimerTick(string timeString)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                int activeIndex = _players.FindIndex(p => p.Name == _currentTurnPlayer);
                if (activeIndex >= 0 && activeIndex < _playerTimes.Length)
                {
                    _playerTimes[activeIndex].Content = $"{Lang.PlayGameMultiplayer_Label_TimeTurn} {timeString}";
                }
            });
        }

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTurnPlayer != UserSession.Username)
            {
                return;
            }

            if (sender is Button button && button.DataContext is Card clickedCard)
            {
                if (clickedCard.IsFlipped || clickedCard.IsMatched)
                {
                    return;
                }

                try
                {
                    await GameServiceManager.Instance.FlipCardAsync(clickedCard.Id);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error sending flip: {ex.Message}");
                    ExceptionManager.Handle(ex, this);
                }
            }
        }

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

                        _playerBorders[i].BorderBrush = Brushes.Gold;
                        _playerBorders[i].BorderThickness = new Thickness(3);
                    }
                    else
                    {
                        _playerNames[i].Foreground = Brushes.White;
                        _playerNames[i].FontWeight = FontWeights.Normal;

                        _playerBorders[i].BorderBrush = Brushes.Transparent;
                        _playerBorders[i].BorderThickness = new Thickness(1);
                    }
                }
            }
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

        #endregion

        #region Chat & Interaction

        private void OnChatMessageReceived(string sender, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                if (!this.IsLoaded)
                {
                    return;
                }

                string formattedMsg = isNotification ? $"--- {message} ---" : $"{sender}: {message}";
                ChatListBox.Items.Add(formattedMsg);

                if (ChatListBox.Items.Count > MAX_CHAT_MESSAGES)
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
                if (!this.IsLoaded)
                {
                    return;
                }

                ChatListBox.Items.Add($"--- {playerName} left the game ---");
                int index = _players.FindIndex(p => p.Name == playerName);

                if (index >= 0 && index < _playerBorders.Length)
                {
                    _playerBorders[index].Opacity = 0.5;
                    _playerNames[index].Content += " (Left)";
                    _playerBorders[index].ContextMenu = null;
                }
            });
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {
            string message = ChatTextBox.Text?.Trim();


            if (message.Length > MAX_CHAT_MESSAGES)
            {
                CustomMessageBox messageBox = new CustomMessageBox(Lang.Global_Title_Warning, Lang.Lobby_Message_InvalidNumberChar, this, MessageBoxType.Warning);
                messageBox.ShowDialog();
                return;
            }

            if (!string.IsNullOrWhiteSpace(ChatTextBox.Text))
            {
                try
                {
                    await GameServiceManager.Instance.SendChatMessageAsync(ChatTextBox.Text);
                    ChatTextBox.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Chat Error: {ex.Message}");
                }
            }
        }

        private async void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_LeaveLobby, Lang.Global_Message_ExitGame,
                this, ConfirmationMessageBox.ConfirmationBoxType.Warning);

            if (confirmationBox.ShowDialog() == true)
            {
                await LeaveGameSafe();
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.ShowDialog(this, new Settings());
        }

        #endregion

        #region Vote Kick & Cleanup

        private async void VoteKick_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem &&
                menuItem.Parent is ContextMenu contextMenu &&
                contextMenu.PlacementTarget is Border targetBorder &&
                targetBorder.Tag is string targetPlayerName)
            {
                if (targetPlayerName == UserSession.Username)
                {
                    new CustomMessageBox(Lang.Global_Title_Warning, Lang.KickVote_Message_CantVoteYourself, this, MessageBoxType.Warning).ShowDialog();
                    return;
                }

                string messageVote = string.Format(Lang.KickVote_Message_VoteKickPlayer, targetPlayerName);
                var confirmation = new ConfirmationMessageBox(Lang.KickVote_Title_ConfirmVote, messageVote, this, ConfirmationMessageBox.ConfirmationBoxType.Question);

                if (confirmation.ShowDialog() == true)
                {
                    try
                    {
                        await GameServiceManager.Instance.VoteToKickAsync(targetPlayerName);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error voting kick: {ex.Message}");
                        ExceptionManager.Handle(ex, this);
                    }
                }
            }
        }

        private void ShowMatchSummary(string title, string stats)
        {
            var summary = new MatchSummary(title, stats);
            NavigationHelper.ShowDialog(this, summary);

            _ = LeaveGameSafe();
        }

        private async Task LeaveGameSafe()
        {
            UnsubscribeEvents();
            _gameManager.StopGame();

            try
            {
                await GameServiceManager.Instance.LeaveLobbyAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error leaving game: {ex.Message}");
            }
            finally
            {
                NavigationHelper.NavigateTo(this, new MultiplayerMenu());
            }
        }

        protected override async void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
            _gameManager.StopGame();

            if (GameServiceManager.Instance.Client.State == System.ServiceModel.CommunicationState.Opened)
            {
                try
                {
                    await GameServiceManager.Instance.LeaveLobbyAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[PlayGameMultiplayer] Error leaving lobby: {ex.Message}");
                }
            }

            try
            {
                if (this.Owner != null && Application.Current.MainWindow != this.Owner)
                {
                    this.Owner.Show();
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"[PlayGameMultiplayer] Could not show owner: {ex.Message}");
            }

            base.OnClosed(e);
        }

        #endregion
    }
}