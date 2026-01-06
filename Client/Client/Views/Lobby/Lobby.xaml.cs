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
    /// Logic for Lobby.xaml. Manages guest connection and waiting room.
    /// </summary>
    public partial class Lobby : Window
    {
        #region Private Fields
        private readonly string _lobbyCode;
        private bool _isConnected = false;
        private readonly Label[] _playerLabels;
        private bool _isGameStarting = false;
        private List<LobbyPlayerInfo> _currentPlayers = new List<LobbyPlayerInfo>();
        #endregion

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

        #region Event Configuration

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
                bool success;

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
                    new CustomMessageBox(Lang.Global_Title_Success, Lang.Lobby_Notification_PlayerJoined, this, MessageBoxType.Information).ShowDialog();
                }
                else
                {
                    new CustomMessageBox(Lang.Global_Title_Error, Lang.Lobby_Error_JoinFailed, this, MessageBoxType.Error).ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, async () =>
                {
                    await LeaveLobbySafe();
                    this.Close();
                });
            }
        }

        #region Server Callbacks

        private void OnGameStarted(List<CardInfo> cards)
        {
            Dispatcher.Invoke(() =>
            {
                UnsubscribeEvents();
                _isGameStarting = true;

                var gameWindow = new PlayGameMultiplayer(cards, _currentPlayers);

                if (this.Owner != null)
                {
                    gameWindow.Owner = this.Owner;
                }

                gameWindow.Show();
                this.Close();
            });
        }

        private void OnPlayerListUpdated(LobbyPlayerInfo[] players)
        {
            Dispatcher.Invoke(() =>
            {
                _currentPlayers = players.ToList();
                UpdatePlayerLabels();
            });
        }

        private void OnPlayerLeft(string name)
        {
            Dispatcher.Invoke(() =>
            {
                var p = _currentPlayers.FirstOrDefault(x => x.Name == name);
                if (p != null)
                {
                    _currentPlayers.Remove(p);
                    UpdatePlayerLabels();
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

        #endregion

        #region UI Interactions

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
                    ExceptionManager.Handle(ex, this, null);
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
            if (sender is Button button)
            {
                button.IsEnabled = false;
                button.Content = "Ready!";
            }
        }

        private async void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            await LeaveLobbySafe();
            this.Close();
        }

        private void UpdatePlayerLabels()
        {
            foreach (var label in _playerLabels)
            {
                if (label != null) label.Content = "";
            }
            for (int i = 0; i < _currentPlayers.Count && i < _playerLabels.Length; i++)
            {
                if (_playerLabels[i] != null)
                {
                    _playerLabels[i].Content = _currentPlayers[i].Name;
                }
            }
        }

        #endregion

        #region Cleanup

        protected override async void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();

            if (!_isGameStarting)
            {
                await LeaveLobbySafe();

                if (this.Owner != null)
                {
                    this.Owner.Show();
                }
            }
            base.OnClosed(e);
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
                    Debug.WriteLine($"[LeaveLobbySafe Warning]: {ex.Message}");
                }
            }
        }

        private void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void PlayersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        #endregion
    }
}