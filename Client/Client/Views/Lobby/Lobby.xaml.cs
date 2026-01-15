using Client.Core;
using Client.Core.Exceptions;
using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
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
using System.Windows.Interop;
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
            bool joinedSuccessfully = await ExceptionManager.ExecuteNetworkCallAsync(async () =>
            {
                var sessionCheck = await UserServiceManager.Instance.RenewSessionAsync(UserSession.SessionToken);

                if (!sessionCheck.Success)
                {
                    throw new FaultException(sessionCheck.MessageKey);
                }

                bool result;
                if (UserSession.IsGuest)
                {
                    result = await GameServiceManager.Instance.JoinLobbyAsync(
                        UserSession.SessionToken, _lobbyCode, true, UserSession.Username);
                }
                else
                {
                    result = await GameServiceManager.Instance.JoinLobbyAsync(
                        UserSession.SessionToken, _lobbyCode, false, null);
                }

                if (!result)
                {
                    throw new Exception(Lang.Lobby_Error_JoinFailed);
                }
            }, this, NetworkFailPolicy.ShowWarningOnly);

            if (joinedSuccessfully)
            {
                _isConnected = true;

                if (!_currentPlayers.Any(p => p.Name == UserSession.Username))
                {
                    _currentPlayers.Add(new LobbyPlayerInfo { Name = UserSession.Username });
                    UpdatePlayerUI();
                }

                string message = string.Format(Lang.Lobby_Notification_PlayerJoined, UserSession.Username);
                OnChatMessageReceived(Lang.Global_Label_System, message, true);
            }
            else
            {
                GoBackToMenu();
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

        private void ButtonInvite_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.ShowDialog(this, new InviteFriendDialog(_lobbyCode));
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {
            if (ChatTextBox != null && !string.IsNullOrWhiteSpace(ChatTextBox.Text))
            {
                string message = ChatTextBox.Text;
                ChatTextBox.Text = string.Empty;

                await ExceptionManager.ExecuteNetworkCallAsync(async () =>
                {
                    await GameServiceManager.Instance.SendChatMessageAsync(message);
                }, this, NetworkFailPolicy.ShowWarningOnly);
            }
        }

        private async void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationMessageBox confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_LeaveLobby, Lang.Lobby_Message_LeaveLobby,
                this, ConfirmationMessageBox.ConfirmationBoxType.Warning);

            if (confirmationBox.ShowDialog() == true)
            {
                await LeaveLobbySafe();
                GoBackToMenu();
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

                NavigationHelper.NavigateTo(this, gameWindow);
            });
        }

        private void OnPlayerLeft(string name)
        {
            Dispatcher.Invoke(() =>
            {
                var player = _currentPlayers.FirstOrDefault(x => x.Name == name);
                if (player != null)
                {
                    _currentPlayers.Remove(player);
                    UpdatePlayerUI();
                    string message = string.Format(Lang.Lobby_Notification_PlayerLeft, UserSession.Username);
                    OnChatMessageReceived(Lang.Global_Label_System, message, true);
                }
            });
        }

        protected override async void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();

            if (!_isGameStarting)
            {
                await LeaveLobbySafe();

                if (this.Owner != null && Application.Current.MainWindow != this.Owner)
                {
                    this.Owner.Show();
                }
            }

            base.OnClosed(e);
        }

        private void GoBackToMenu()
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new MultiplayerMenu());
        }

        private async Task LeaveLobbySafe()
        {
            if (_isConnected)
            {
                try
                {
                    await GameServiceManager.Instance.LeaveLobbyAsync();
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