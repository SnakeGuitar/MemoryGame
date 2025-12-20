using Client.GameLobbyServiceReference;
using Client.Properties.Langs;
using Client.Utilities;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Client.Views.Multiplayer
{
    /// <summary>
    /// Lógica de interacción para PlayGameMultiplayer.xaml
    /// </summary>
    public partial class PlayGameMultiplayer : Window
    {
        public ObservableCollection<Card> Cards { get; set; }
        public readonly List<LobbyPlayerInfo> _players;
        public readonly List<CardInfo> _initialBoard;

        private DispatcherTimer _uiTurnTimer;
        private int _remainingTimeSeconds;
        private string _currentActivePlayerName;

        public PlayGameMultiplayer(List<CardInfo> board, List<LobbyPlayerInfo> players)
        {
            InitializeComponent();

            _initialBoard = board;
            _players = players;

            InitializeGame();
            InitializeTimer();
            ConfigureGameEvents();
        }

        private void InitializeTimer()
        {
            _uiTurnTimer = new DispatcherTimer();
            _uiTurnTimer.Interval = TimeSpan.FromSeconds(1);
            _uiTurnTimer.Tick += OnUiTimerTick;
        }

        private void InitializeGame()
        {
            BorderP1.Visibility = Visibility.Collapsed;
            BorderP2.Visibility = Visibility.Collapsed;
            BorderP3.Visibility = Visibility.Collapsed;
            BorderP4.Visibility = Visibility.Collapsed;

            if (_players != null && _players.Count > 0)
            {
                if (_players.Count >= 1)
                {
                    LabelP1Name.Content = _players[0].Name;
                    LabelP1Score.Content = $"{Lang.Global_Label_Score}: 0";
                    LabelP1Time.Content = "--";
                    BorderP1.Visibility = Visibility.Visible;
                }

                if (_players.Count >= 2)
                {
                    LabelP2Name.Content = _players[1].Name;
                    LabelP2Score.Content = $"{Lang.Global_Label_Score}: 0";
                    LabelP2Time.Content = "--";
                    BorderP2.Visibility = Visibility.Visible;
                }

                if (_players.Count >= 3)
                {
                    LabelP3Name.Content = _players[2].Name;
                    LabelP3Score.Content = $"{Lang.Global_Label_Score}: 0";
                    LabelP3Time.Content = "--";
                    BorderP3.Visibility = Visibility.Visible;
                }

                if (_players.Count >= 4)
                {
                    LabelP4Name.Content = _players[3].Name;
                    LabelP4Score.Content = $"{Lang.Global_Label_Score}: 0";
                    LabelP4Time.Content = "--";
                    BorderP4.Visibility = Visibility.Visible;
                }
            }


            Cards = new ObservableCollection<Card>();

            for (int i = 0; i < _initialBoard.Count; i++)
            {
                var cardInfo = _initialBoard[i];
                var newCard = new Card(i, cardInfo.CardId, string.Empty);
                Cards.Add(newCard);
            }

            GameBoard.ItemsSource = Cards;
        }

        private void ConfigureGameEvents()
        {
            GameServiceManager.Instance.CardShown += OnCardShown;
            GameServiceManager.Instance.CardsHidden += OnCardsHidden;
            GameServiceManager.Instance.CardsMatched += OnCardsMatched;
            GameServiceManager.Instance.TurnUpdated += OnTurnUpdated;
            GameServiceManager.Instance.ScoreUpdated += OnScoreUpdated;
            GameServiceManager.Instance.ChatMessageReceived += OnChatMessageReceived;
            GameServiceManager.Instance.GameFinished += OnGameFinished;
            GameServiceManager.Instance.PlayerLeft += OnPlayerLeft;
        }

        private void UnsubscribeEvents()
        {
            GameServiceManager.Instance.CardShown -= OnCardShown;
            GameServiceManager.Instance.CardsHidden -= OnCardsHidden;
            GameServiceManager.Instance.CardsMatched -= OnCardsMatched;
            GameServiceManager.Instance.TurnUpdated -= OnTurnUpdated;
            GameServiceManager.Instance.ScoreUpdated -= OnScoreUpdated;
            GameServiceManager.Instance.ChatMessageReceived -= OnChatMessageReceived;
            GameServiceManager.Instance.GameFinished -= OnGameFinished;
            GameServiceManager.Instance.PlayerLeft -= OnPlayerLeft;
            _uiTurnTimer.Stop();
        }

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int cardId)
            {
                var card = Cards.FirstOrDefault(c => c.Id == cardId);

                if (card != null && !card.IsFlipped && !card.IsMatched)
                {
                    try
                    {
                        await GameServiceManager.Instance.Client.FlipCardAsync(cardId);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message); // CHANGE WHEN POSSIBLE
                    }
                }
            }
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ChatTextBox.Text))
            {
                string msgToSend = ChatTextBox.Text;
                ChatTextBox.Text = string.Empty;

                try
                {
                    await GameServiceManager.Instance.Client.SendChatMessageAsync(msgToSend);
                }
                catch (Exception)
                {
                }
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Exit?",
                                         "Do you want to leave?",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                CloseGameAndLeave();
            }
        }

        private void OnUiTimerTick(object sender, EventArgs e)
        {
            if (_remainingTimeSeconds > 0)
            {
                _remainingTimeSeconds--;
                UpdateAllPlayerActiveStates(_currentActivePlayerName, _remainingTimeSeconds);
            }
            else
            {
                _uiTurnTimer.Stop();
            }
        }

        private void UpdateAllPlayerActiveStates(string activePlayer, int time)
        {
            UpdateSinglePlayerState(LabelP1Name, LabelP1Time, activePlayer, time);
            UpdateSinglePlayerState(LabelP2Name, LabelP2Time, activePlayer, time);
            UpdateSinglePlayerState(LabelP3Name, LabelP3Time, activePlayer, time);
            UpdateSinglePlayerState(LabelP4Name, LabelP4Time, activePlayer, time);
        }

        private void UpdateSinglePlayerState(Label nameLabel, Label timeLabel, string activePlayer, int time)
        {
            bool isActive = nameLabel.Content?.ToString() == activePlayer;
            nameLabel.Foreground = isActive ? Brushes.Gold : Brushes.White;
            nameLabel.FontWeight = isActive ? FontWeights.Bold : FontWeights.Normal;
            timeLabel.Content = isActive ? $"{Lang.Global_Label_Time}: {time}s" : "--";
        }

        private void OnCardShown(int cardIndex, string imageIdentifier)
        {
            Dispatcher.Invoke(() =>
            {
                var card = Cards.FirstOrDefault(c => c.Id == cardIndex);
                if (card != null)
                {
                    string fullPath = $"/Client;component/Resources/Images/Cards/Fronts/Color/{imageIdentifier}.png";

                    card.SetFrontImage(fullPath);
                    card.IsFlipped = true;
                }
            });
        }

        private void OnCardsHidden(int index1, int index2)
        {
            Dispatcher.Invoke(() =>
            {
                var c1 = Cards.FirstOrDefault(c => c.Id == index1);
                var c2 = Cards.FirstOrDefault(c => c.Id == index2);

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

        private void OnCardsMatched(int index1, int index2)
        {
            Dispatcher.Invoke(() =>
            {
                var c1 = Cards.FirstOrDefault(c => c.Id == index1);
                var c2 = Cards.FirstOrDefault(c => c.Id == index2);

                if (c1 != null)
                {
                    c1.IsMatched = true;
                }
                if (c2 != null)
                {
                    c2.IsMatched = true;
                }
            });
        }

        private void OnTurnUpdated(string currentPlayerName, int timeLeft)
        {
            Dispatcher.Invoke(() =>
            {
                UpdatePlayerActiveState(LabelP1Name, LabelP1Time, currentPlayerName, timeLeft);
                UpdatePlayerActiveState(LabelP2Name, LabelP2Time, currentPlayerName, timeLeft);
                UpdatePlayerActiveState(LabelP3Name, LabelP3Time, currentPlayerName, timeLeft);
                UpdatePlayerActiveState(LabelP4Name, LabelP4Time, currentPlayerName, timeLeft);
                
                _currentActivePlayerName = currentPlayerName;
                _remainingTimeSeconds = timeLeft;
                _uiTurnTimer.Stop();
                _uiTurnTimer.Start();
                UpdateAllPlayerActiveStates(currentPlayerName, timeLeft);
            });
        }

        private void UpdatePlayerActiveState(Label nameLabel, Label timeLabel, string activePlayerName, int time)
        {
            bool isActive = nameLabel.Content.ToString() == activePlayerName;

            if (isActive)
            {
                nameLabel.Foreground = Brushes.Gold;
                nameLabel.FontWeight = FontWeights.Bold;
                timeLabel.Content = $"{Lang.Global_Label_Time}: {time}s";
            }
            else
            {
                nameLabel.Foreground = Brushes.White;
                nameLabel.FontWeight = FontWeights.Normal;
                timeLabel.Content = "--";
            }
        }

        private void OnScoreUpdated(string playerName, int newScore)
        {
            Dispatcher.Invoke(() =>
            {
                string scoreText = $"{Lang.Global_Label_Score}: {newScore}";

                if (LabelP1Name.Content.ToString() == playerName)
                {
                    LabelP1Score.Content = scoreText;
                }
                else if (LabelP2Name.Content.ToString() == playerName)
                {
                    LabelP2Score.Content = scoreText;
                }
                else if (LabelP3Name.Content.ToString() == playerName)
                {
                    LabelP3Score.Content = scoreText;
                }
                else if (LabelP4Name.Content.ToString() == playerName)
                {
                    LabelP4Score.Content = scoreText;
                }
            });
        }

        private void OnChatMessageReceived(string sender, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                string displayMessage = isNotification
                    ? $"--- {message} ---"
                    : $"{sender}: {message}";

                ChatListBox.Items.Add(displayMessage);

                if (ChatListBox.Items.Count > 0)
                {
                    ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
                }
            });
        }

        private void OnGameFinished(string winnerName)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"Winner is: {winnerName}",
                                "Winner",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                CloseGameAndLeave();
            });
        }

        private void OnPlayerLeft(string playerName)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show($"{playerName} left",
                                "Player left",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                CloseGameAndLeave();
            });
        }

        private async void CloseGameAndLeave()
        {
            UnsubscribeEvents();

            try
            {
                await GameServiceManager.Instance.Client.LeaveLobbyAsync();
            }
            catch
            {
            }

            if (this.Owner != null)
            {
                this.Owner.Show();
                this.Owner.WindowState = WindowState.Normal;
            }

            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
            base.OnClosed(e);
        }
    }
}
