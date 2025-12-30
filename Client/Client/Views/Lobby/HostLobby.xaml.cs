using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Utilities;
using Client.Views.Multiplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using static Client.Helpers.ValidationHelper;

namespace Client.Views.Lobby
{
    /// <summary>
    /// Lógica de interacción para HostLobby.xaml
    /// </summary>
    public partial class HostLobby : Window
    {
        private readonly String _lobbyCode;
        private bool _isConnected = false;

        private readonly Label[] _playerLabels;

        private List<LobbyPlayerInfo> _currentPlayers = new List<LobbyPlayerInfo>();
        private bool _isGameStarting = false;

        private int _selectedCardCount = 16;
        private int _secondsPerTurn = 30;

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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = await GameServiceManager.Instance.Client.JoinLobbyAsync(
                    UserSession.SessionToken,
                    _lobbyCode,
                    false,
                    null
                    );

                if (success)
                {
                    _isConnected = true;
                    var msgBox = new Controls.CustomMessageBox(
                        Lang.Global_Title_Success,
                        Lang.Lobby_Message_CreateSuccess,
                        this, Controls.CustomMessageBox.MessageBoxType.Success);
                    msgBox.ShowDialog();
                }
                else
                {
                    var msgBox = new Controls.CustomMessageBox(
                        Lang.Global_Title_Error,
                        Lang.HostLobby_Error_CreateFailed,
                        this, Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();
                    this.Close();
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new Controls.CustomMessageBox(
                    Lang.Global_Title_ServerOffline, errorMessage,
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
            }
        }

        private void ButtonStartMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!_isConnected)
            {
                return;
            }

            try
            {
                if (_selectedCardCount < 4 || _selectedCardCount > 36 || _selectedCardCount % 2 != 0)
                {
                    _selectedCardCount = 16;
                }

                if (_secondsPerTurn < 5)
                { 
                    _secondsPerTurn = 10;
                }

                var settings = new GameSettings
                {
                    CardCount = _selectedCardCount,
                    TurnTimeSeconds = _secondsPerTurn
                };
                GameServiceManager.Instance.Client.StartGame(settings);
            }
            catch (FaultException ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
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

        }

        private void OnGameStarted(List<CardInfo> cards)
        {
            Dispatcher.Invoke(() =>
            {
                UnsubscribeEvents();
                _isGameStarting = true;

                var multiplayerGame = new PlayGameMultiplayer(cards, _currentPlayers);
                multiplayerGame.Owner = this;
                multiplayerGame.Show();
                this.Hide();
            });
        }

        protected override async void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();

            if (this.Owner != null)
            {
                this.Owner.Show();
            }

            if (_isConnected && !_isGameStarting)
            {
                try
                {
                    await GameServiceManager.Instance.Client.LeaveLobbyAsync();
                }
                catch { }
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
    }
}

