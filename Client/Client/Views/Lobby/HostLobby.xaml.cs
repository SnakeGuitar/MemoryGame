using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Utilities;
using Client.Views.Controls;
using Client.Views.Multiplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Lobby
{
    /// <summary>
    /// Lógica de interacción para HostLobby.xaml
    /// </summary>
    public partial class HostLobby : Window
    {
        #region
        private readonly String _lobbyCode;
        private bool _isConnected = false;

        private readonly Label[] _playerLabels;

        private List<LobbyPlayerInfo> _currentPlayers = new List<LobbyPlayerInfo>();
        private bool _isGameStarting = false;

        private int _selectedCardCount = GameConstants.DefaultCardCount;
        private int _secondsPerTurn = GameConstants.DefaultTurnTimeSeconds;
        #endregion

        public HostLobby()
        {
            InitializeComponent();

            _lobbyCode = ClientHelper.GenerateGameCode();

            if (FindName("LabelGameCode") is Label labelGameCode)
            {
                labelGameCode.Content = _lobbyCode;
            }

            _playerLabels = new Label[]
            {
                FindName("LabelPlayer1") as Label,
                FindName("LabelPlayer2") as Label,
                FindName("LabelPlayer3") as Label,
                FindName("LabelPlayer4") as Label
            };

            ConfigureEvents();
        }

        #region Event configuration
        private void ConfigureEvents()
        {
            GameServiceManager.Instance.PlayerListUpdated += OnPlayerListUpdated;
            GameServiceManager.Instance.GameStarted += OnGameStarted;
            GameServiceManager.Instance.ChatMessageReceived += OnChatMessageReceived;
            GameServiceManager.Instance.PlayerLeft += OnPlayerLeft;
        }

        private void UnsubscribeEvents()
        {
            GameServiceManager.Instance.PlayerListUpdated -= OnPlayerListUpdated;
            GameServiceManager.Instance.GameStarted -= OnGameStarted;
            GameServiceManager.Instance.ChatMessageReceived -= OnChatMessageReceived;
            GameServiceManager.Instance.PlayerLeft -= OnPlayerLeft;
        }
        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = await GameServiceManager.Instance.Client.CreateLobbyAsync(
                    UserSession.SessionToken,
                    _lobbyCode
                    );

                if (success)
                {
                    _isConnected = true;
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.Lobby_Message_CreateSuccess,
                        this, MessageBoxType.Success);
                    msgBox.ShowDialog();
                }
                else
                {
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, Lang.HostLobby_Error_CreateFailed,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
        }

        private void ButtonStartMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!_isConnected)
            {
                return;
            }

            if (_currentPlayers.Count < GameConstants.MinPlayersToPlay)
            {
                MessageBox.Show(
                    "Waiting for more players...",
                    "Notice",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            if (_selectedCardCount < GameConstants.MinCardCount ||
                _selectedCardCount > GameConstants.MaxCardCount ||
                _selectedCardCount % 2 != 0)
            {
                _selectedCardCount = GameConstants.DefaultCardCount;
            }

            if (_secondsPerTurn < GameConstants.MinTurnTimeSeconds)
            {
                _secondsPerTurn = GameConstants.MinTurnTimeSecondsFallback;
            }

            try
            {
                var settings = new GameSettings
                {
                    CardCount = _selectedCardCount,
                    TurnTimeSeconds = _secondsPerTurn
                };
                GameServiceManager.Instance.Client.StartGame(settings);
            }
            catch (Exception ex)
            {
                HandleConnectionError(ex);
            }
        }

        private async void ButtonSendMessageToChat_Click(object sender, RoutedEventArgs e)
        {
            if (FindName("ChatTextBox") is TextBox txtMsg && !string.IsNullOrWhiteSpace(txtMsg.Text))
            {
                string msgToSend = txtMsg.Text;
                txtMsg.Text = string.Empty;

                try
                {
                    await GameServiceManager.Instance.Client.SendChatMessageAsync(msgToSend);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error enviando mensaje: " + ex.Message);
                }
            }
        }

        private void ButtonSendInvitationToFriend_Click(object sender, RoutedEventArgs e)
        {
            string friendEmail = TextBoxFriendEmail.Text.Trim();

            if (string.IsNullOrEmpty(friendEmail))
            {
                MessageBox.Show("Please enter an email address.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string subject = "Join my Memory Game Lobby!";
                string body = $"Join me! The lobby code is: {_lobbyCode}";
                string mailtoUri = $"mailto:{friendEmail}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

                Process.Start(new ProcessStartInfo(mailtoUri) { UseShellExecute = true });

                MessageBox.Show($"Invitation opened for {friendEmail}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                TextBoxFriendEmail.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open mail client: " + ex.Message);
            }
        }

        private async void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_isConnected)
            {
                try
                {
                    await GameServiceManager.Instance.Client.LeaveLobbyAsync();
                    _isConnected = false;
                }
                catch { }
            }
            this.Close();
        }

        private void OnPlayerListUpdated(LobbyPlayerInfo[] players)
        {
            Dispatcher.Invoke(() =>
            {
                _currentPlayers = players.ToList();

                foreach (var label in _playerLabels)
                {
                    if (label != null) label.Content = string.Empty;
                }

                for (int i = 0; i < players.Length && i < _playerLabels.Length; i++)
                {
                    if (_playerLabels[i] != null)
                    {
                        _playerLabels[i].Content = players[i].Name;
                    }
                }
            });
        }

        private void OnChatMessageReceived(string sender, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                if (FindName("ChatListBox") is ListBox chatBox)
                {
                    string formattedMessage = isNotification ? $"--- {message} ---" : $"{sender}: {message}";
                    chatBox.Items.Add(formattedMessage);

                    if (chatBox.Items.Count > 0)
                    {
                        chatBox.ScrollIntoView(chatBox.Items[chatBox.Items.Count - 1]);
                    }
                }
            });
        }

        private void OnPlayerLeft(string playerName)
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    $"{playerName} has left the lobby.", "Player Left",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                var playerToRemove = _currentPlayers.FirstOrDefault(p => p.Name == playerName);
                if (playerToRemove != null)
                {
                    _currentPlayers.Remove(playerToRemove);
                    UpdatePlayerLabels();
                }
            });
        }

        private void OnGameStarted(List<CardInfo> cards)
        {
            Dispatcher.Invoke(() =>
            {
                UnsubscribeEvents();
                _isGameStarting = true;

                var multiplayerGame = new PlayGameMultiplayer(cards, _currentPlayers);
                if (this.Owner != null)
                {
                    multiplayerGame.Owner = this.Owner;
                }
                else
                {
                    multiplayerGame.Owner = this;
                }
                multiplayerGame.Show();
                this.Hide();
            });
        }

        protected override async void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();

            if (!_isGameStarting)
            {
                try
                {
                    await LeaveLobbySafe();
                    _isConnected = false;
                }
                catch { }
            }

            if (this.Owner != null)
            {
                this.Owner.Show();
            }

            base.OnClosed(e);
        }

        private void ComboBoxNumberOfCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxNumberOfCards.SelectedIndex >= 0)
            {
                var selectedText = ComboBoxNumberOfCards.SelectedItem.ToString();
                _selectedCardCount = int.Parse(selectedText);
            }

        }

        private void SliderSecondsPerTurn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelTimerValue != null)
            {
                LabelTimerValue.Content = e.NewValue.ToString("F0");
            }

            _secondsPerTurn = (int)SliderSecondsPerTurn.Value;
        }

        private void PlayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void UpdatePlayerLabels()
        {
            foreach (var label in _playerLabels)
            {
                if (label != null)
                {
                    label.Content = string.Empty;
                }
            }
            for (int i = 0; i < _currentPlayers.Count && i < _playerLabels.Length; i++)
            {
                if (_playerLabels[i] != null)
                {
                    _playerLabels[i].Content = _currentPlayers[i].Name;
                }
            }
        }

        private async Task LeaveLobbySafe()
        {
            if (_isConnected)
            {
                try
                {
                    await GameServiceManager.Instance.Client.LeaveLobbyAsync();
                    _isConnected = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[LeaveLobbySafe Error]: {ex.ToString()}");
                }
            }
        }

        private void HandleConnectionError(Exception ex)
        {
            string title = Lang.Global_Title_AppError;
            switch (ex)
            {
                case EndpointNotFoundException _:
                    title = Lang.Global_Title_ServerOffline;
                    break;

                case CommunicationException _:
                    title = Lang.Global_Title_NetworkError;
                    break;

                case TimeoutException _:
                    title = Lang.Global_Title_NetworkError;
                    break;

                default:
                    title = Lang.Global_Title_AppError;
                    break;
            }
            string message = LocalizationHelper.GetString(ex);
            Debug.WriteLine($"[{title} - {ex.GetType().Name}]: {ex.Message}");
            new CustomMessageBox(title, message, this, MessageBoxType.Error).ShowDialog();
            this.Close();
        }
    }
}