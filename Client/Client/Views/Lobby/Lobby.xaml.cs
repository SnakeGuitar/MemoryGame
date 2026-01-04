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
using System.Windows;
using System.Windows.Controls;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Lobby
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        private readonly string _lobbyCode;
        private bool _isConnected = false;
        private readonly Label[] _playerLabels;

        private bool _isGameStarting = false;
        private List<LobbyPlayerInfo> _currentPlayers = new List<LobbyPlayerInfo>();

        public Lobby(string lobbyCode)
        {
            InitializeComponent();
            _lobbyCode = lobbyCode;

            if (FindName("LabelLobbyCode") is Label label)
            {
                label.Content = _lobbyCode;
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
                bool success = false;

                if (UserSession.IsGuest)
                {
                    success = await GameServiceManager.Instance.Client.JoinLobbyAsync(
                        UserSession.SessionToken,
                        _lobbyCode,
                        true,
                        UserSession.Username
                    );
                }
                else
                {
                    success = await GameServiceManager.Instance.Client.JoinLobbyAsync(
                        UserSession.SessionToken,
                        _lobbyCode,
                        false,
                        null
                    );
                }

                if (success)
                {
                    _isConnected = true;
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.Lobby_Notification_PlayerJoined,
                        this, MessageBoxType.Information);
                    msgBox.ShowDialog();
                }
                else
                {
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, Lang.Lobby_Error_JoinFailed,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    this.Close();
                }

            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_Error, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonBackToMainMenu_Click(this, new RoutedEventArgs());
            }
            catch (CommunicationException ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonBackToMainMenu_Click(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonBackToMainMenu_Click(this, new RoutedEventArgs());
            }
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
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

        private void ButtonInvite_Click(object sender, RoutedEventArgs e)
        {
            var inviteDialog = new InviteFriendDialog(_lobbyCode);
            inviteDialog.Owner = this;
            inviteDialog.ShowDialog();
        }

        private void ButtonReady_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
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

        private void OnPlayerLeft(string name)
        {
        }

        private void OnGameStarted(List<CardInfo> cards)
        {
            Dispatcher.Invoke(() =>
            {
                _isGameStarting = true;
                UnsubscribeEvents();

                PlayGameMultiplayer gameWindow = new PlayGameMultiplayer(cards, _currentPlayers);

                if (this.Owner != null)
                {
                    gameWindow.Owner = this.Owner;
                }

                gameWindow.Show();
                this.Close();
            });
        }

        protected override async void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();

            if (!_isGameStarting)
            {
                if (this.Owner != null)
                {
                    this.Owner.Show(); // Mostrar ventana anterior
                }

                if (_isConnected)
                {
                    try
                    {
                        await GameServiceManager.Instance.Client.LeaveLobbyAsync();
                        _isConnected = false;
                    }
                    catch { }
                }
            }

            base.OnClosed(e);
        }

        private void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void PlayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
