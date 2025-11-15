using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Windows;

namespace Client.Views.Lobby
{
    /// <summary>
    /// Lógica de interacción para Lobby.xaml
    /// </summary>
    public partial class Lobby : Window, IGameLobbyServiceCallback
    {
        private readonly InstanceContext _callbackContext;
        private readonly GameLobbyServiceClient _lobbyClient;

        private readonly string _lobbyCode;
        private bool isConnected = false;
        public Lobby(string lobbyCode)
        {
            InitializeComponent();
            _lobbyCode = lobbyCode;

            _callbackContext = new InstanceContext(this);
            _lobbyClient = new GameLobbyServiceClient(_callbackContext);

            this.Closed += Window_Closed;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success;
                if (UserSession.IsGuest)
                {
                    success = await _lobbyClient.JoinLobbyAsync(UserSession.SessionToken, _lobbyCode, true, UserSession.Username);
                }
                else
                {
                    success = await _lobbyClient.JoinLobbyAsync(UserSession.SessionToken, _lobbyCode, false, null);
                }

                if (success)
                {
                    isConnected = true;
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Success,
                        Lang.Lobby_Notification_PlayerJoined,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Information);
                    msgBox.ShowDialog();
                }
                else
                {
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error,
                        Lang.Lobby_Error_JoinFailed,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();

                    this.Close();
                }

            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_Error, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonBackToMainMenu_Click(this, new RoutedEventArgs());
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonBackToMainMenu_Click(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonBackToMainMenu_Click(this, new RoutedEventArgs());
            }
        }

        public void PlayerJoined(string playerNmae, bool isGuest)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayerList(LobbyPlayerInfo[] players)
        {
            Dispatcher.Invoke(() =>
            {
                PlayersListBox.Items.Clear();
                foreach (var player in players)
                {
                    PlayersListBox.Items.Add($"{player.Name} {(player.IsGuest ? Lang.Global_Player_GuestSuffix : "")}");
                }
            });
        }
        public void playerLeft(string playerName)
        {
            Dispatcher.Invoke(() =>
            {
                var item = PlayersListBox.Items.Cast<string>()
                .FirstOrDefault(p => p == playerName);
                if (item != null)
                {
                    ChatListBox.Items.Add(string.Format(Lang.Lobby_Notification_PlayerLeft, playerName));
                }
            });
        }
        public void ReceiveChatMessage(string senderName, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                string display = isNotification
                    ? $"{Lang.Lobby_Notification_System} {message}"
                    : $"{senderName}: {message}";
                ChatListBox.Items.Add(display);
                ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
            });
        }

        private async void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.IsGuest)
            {
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NotAvailableFunction,
                    Lang.Lobby_Error_GuestChat,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Warning);
                msgBox.ShowDialog();

                return;
            }

            if (string.IsNullOrWhiteSpace(ChatTextBox.Text))
            {
                return;
            }

            try
            {
                await _lobbyClient.SendChatMessageAsync(ChatTextBox.Text);
                ChatTextBox.Clear();
            }
            catch (CommunicationException ex)
            {
                Debug.WriteLine($"[Chat Send Error]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError,
                    Helpers.LocalizationManager.GetString(ex),
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }

        }


        private void ButtonReady_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Window multiplayerMenu = this.Owner;

            if (multiplayerMenu != null)
            {
                multiplayerMenu.WindowState = this.WindowState;
                multiplayerMenu.Show();
            }
            this.Close();
        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            if (isConnected)
            {
                try
                {
                    await _lobbyClient.LeaveLobbyAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error leaving lobby: {ex.Message}");
                }
            }

            try
            {
                if (_lobbyClient.State != CommunicationState.Faulted)
                {
                    _lobbyClient.Close();
                }
                else
                {
                    _lobbyClient.Abort();
                }
            }
            catch
            {
                _lobbyClient.Abort();
            }
        }


    }
}
