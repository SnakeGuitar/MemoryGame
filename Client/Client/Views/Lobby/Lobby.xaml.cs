using Client.GameLobbyServiceReference;
using Client.Properties.Langs;
using Client.Core;
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
    public partial class Lobby : Window
    {
        #region Private Fields

        private readonly string _lobbyCode;
        private bool _isConnected = false;
        private bool _isGameStarting = false;
        private List<LobbyPlayerInfo> _currentPlayers = new List<LobbyPlayerInfo>();

        #endregion

        public Lobby(string lobbyCode)
        {
            InitializeComponent();
            _lobbyCode = lobbyCode;

            if (LabelLobbyCode != null)
            {
                LabelLobbyCode.Content = _lobbyCode;
            }

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
                string username = UserSession.Username;

                if (UserSession.IsGuest)
                {
                    success = await GameServiceManager.Instance.Client.JoinLobbyAsync(
                        UserSession.SessionToken, _lobbyCode, true, username);
                }
                else
                {
                    success = await GameServiceManager.Instance.Client.JoinLobbyAsync(
                        UserSession.SessionToken, _lobbyCode, false, null);
                }

                if (success)
                {
                    _isConnected = true;

                    if (!_currentPlayers.Any(p => p.Name == username))
                    {
                        _currentPlayers.Add(new LobbyPlayerInfo { Name = username });
                        UpdatePlayerUI();
                    }

                    OnChatMessageReceived("System", $"{username} has joined the lobby.", true);

                    string successMsg = Lang.Lobby_Notification_PlayerJoined.Contains("{0}")
                        ? string.Format(Lang.Lobby_Notification_PlayerJoined, username)
                        : Lang.Lobby_Notification_PlayerJoined;

                    new CustomMessageBox(Lang.Global_Title_Success, successMsg, this, MessageBoxType.Information).ShowDialog();
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

        #region UI Updates

        private void OnPlayerListUpdated(LobbyPlayerInfo[] players)
        {
            Dispatcher.Invoke(() =>
            {
                _currentPlayers = players.ToList();
                UpdatePlayerUI();
            });
        }

        private void UpdatePlayerUI()
        {
            if (PlayersListBox == null)
            {
                return;
            }

            PlayersListBox.Items.Clear();

            foreach (var player in _currentPlayers)
            {
                PlayersListBox.Items.Add(player.Name);
            }
        }

        private void OnChatMessageReceived(string sender, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                if (ChatListBox != null)
                {
                    string formattedMessage = isNotification ? $"--- {message} ---" : $"{sender}: {message}";
                    ChatListBox.Items.Add(formattedMessage);

                    if (ChatListBox.Items.Count > 0)
                    {
                        ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
                    }
                }
            });
        }

        #endregion

        #region Interaction Handlers

        private void ButtonReady_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.IsEnabled = false;
                btn.Content = "Waiting...";
            }
        }

        private void ButtonInvite_Click(object sender, RoutedEventArgs e)
        {
            var inviteDialog = new InviteFriendDialog(_lobbyCode);
            inviteDialog.Owner = this;
            inviteDialog.ShowDialog();
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {
            if (ChatTextBox != null && !string.IsNullOrWhiteSpace(ChatTextBox.Text))
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

        private async void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationMessageBox confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_LeaveLobby, Lang.Lobby_Message_LeaveLobby,
                this, ConfirmationMessageBox.ConfirmationBoxType.Warning);

            bool? result = confirmationBox.ShowDialog();

            if (result == true)
            {
                await LeaveLobbySafe();
                this.Close();
            }
            
        }

        #endregion

        #region Server Callbacks & Cleanup

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

        private void OnPlayerLeft(string name)
        {
            Dispatcher.Invoke(() =>
            {
                var p = _currentPlayers.FirstOrDefault(x => x.Name == name);
                if (p != null)
                {
                    _currentPlayers.Remove(p);
                    UpdatePlayerUI();
                    OnChatMessageReceived("System", $"{name} left the lobby.", true);
                }
            });
        }

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
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[LeaveLobbySafe] Ignored error: {ex.Message}");
                }
                finally
                {
                    _isConnected = false;
                }
            }
        }

        #endregion
    }
}