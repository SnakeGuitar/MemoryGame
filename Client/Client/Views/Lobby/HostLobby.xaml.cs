using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Multiplayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Client.Helpers.ValidationHelper;

namespace Client.Views.Lobby
{
    /// <summary>
    /// Lógica de interacción para HostLobby.xaml
    /// </summary>
    public partial class HostLobby : Window, IGameLobbyServiceCallback
    {
        private readonly InstanceContext _callBackContext;
        private readonly GameLobbyServiceClient _lobbyClient;

        private readonly String _lobbyCode;

        private bool isConnected = false;
        private int selectedCardCount = 16;
        private int secondsPerTurn = 30;
        private readonly Label[] _playerLabels;


        public HostLobby()
        {
            _lobbyCode = Helpers.ClientHelper.GenerateGameCode();

            _callBackContext = new InstanceContext(this);
            _lobbyClient = new GameLobbyServiceClient(_callBackContext);

            InitializeComponent();

            this.Closed += Window_Closed;

            _playerLabels = new Label[]
            {
                LabelPlayer1,
                LabelPlayer2,
                LabelPlayer3,
                LabelPlayer4

            };

            LabelGameCode.Content = _lobbyCode;
            SetupComboBox();
            SliderSecondsPerTurn.Value = secondsPerTurn;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = await _lobbyClient.JoinLobbyAsync(
                    UserSession.SessionToken,
                    _lobbyCode,
                    false,
                    null
                    );
                if (success)
                {
                    isConnected = true;
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Success,
                        Lang.Lobby_Message_CreateSuccess,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Success);
                    msgBox.ShowDialog();
                }
                else
                {
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error,
                        Lang.HostLobby_Error_CreateFailed,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();

                    this.Close();
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_ServerOffline, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                this.Close();
            }
        }

        private void SetupComboBox()
        {
            var cardOptions = new List<string> { "16", "24", "30" };
            ComboBoxNumberOfCards.ItemsSource = cardOptions;
            ComboBoxNumberOfCards.SelectedIndex = 0;
        }

        private void ButtonStartMatch_Click(object sender, RoutedEventArgs e)
        {
            var multiplayerGame = new PlayGameMultiplayer();
            multiplayerGame.WindowState = this.WindowState;
            multiplayerGame.Owner = this;
            multiplayerGame.Show();
            this.Hide();
        }

        private async void ButtonSendMessageToChat_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = this.FindName("TextBoxChat") as TextBox;
            if (messageBox == null || string.IsNullOrWhiteSpace(messageBox.Text))
            {
                return;
            }

            try
            {
                await _lobbyClient.SendChatMessageAsync(messageBox.Text);
                messageBox.Clear();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Chat Error]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }

        }

        private void ButtonSendInvitationToFriend_Click(object sender, RoutedEventArgs e)
        {
            string friendEmail = TextBoxFriendEmail.Text.Trim();

            ValidationCode validationCode = Helpers.ValidationHelper.ValidateEmail(friendEmail);

            if (validationCode != ValidationCode.Success)
            {
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_Error, Helpers.LocalizationHelper.GetString(validationCode),
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Warning);
                msgBox.ShowDialog();

                return;
            }

            if (Helpers.EmailHelper.SendInvitationEmail(friendEmail, _lobbyCode))
            {
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_Success,
                    string.Format(Lang.HostLobby_Message_InviteSent, friendEmail),
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Success);
                msgBox.ShowDialog();
            }
            else
            {
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_Error,
                    string.Format(Lang.HostLobby_Error_InviteFailed, friendEmail),
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }

        private void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Window multiplayerMenu = this.Owner;

            if (multiplayerMenu != null)
            {
                multiplayerMenu.WindowState = this.WindowState;
                multiplayerMenu.Show();
            }
            this.Close();
        }

        private void ComboBoxNumberOfCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxNumberOfCards.SelectedIndex >= 0)
            {
                var selectedText = ComboBoxNumberOfCards.SelectedItem.ToString();
                selectedCardCount = int.Parse(selectedText);
            }

        }

        private void SliderSecondsPerTurn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelTimerValue != null)
            {
                LabelTimerValue.Content = e.NewValue.ToString("F0");
            }

            secondsPerTurn = (int)SliderSecondsPerTurn.Value;
        }

        private void ChatListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // LOBBYCALLBACKS

        public void ReceiveChatMessage(string senderName, string message, bool isNotification)
        {
            Dispatcher.Invoke(() =>
            {
                // 5. I18N
                string displayMessage = isNotification ?
                    $"{Lang.Lobby_Notification_System} {message}" :
                    $"{senderName}: {message}";
                ChatListBox.Items.Add(displayMessage);
                ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
            });
        }

        public void UpdatePlayerList(LobbyPlayerInfo[] players)
        {
            Dispatcher.Invoke(() =>
            {
                foreach (var label in _playerLabels)
                {
                    label.Content = "";
                    label.Visibility = Visibility.Collapsed;
                }

                for (int i = 0; i < Math.Min(players.Length, _playerLabels.Length); i++)
                {
                    var player = players[i];
                    _playerLabels[i].Content = $"{player.Name} {(player.IsGuest ? Lang.Global_Player_GuestSuffix : "")}";
                    _playerLabels[i].Visibility = Visibility.Visible;
                }
            });
        }

        // END LOBBYCALLBACKS

        public void PlayerJoined(string playerNmae, bool isGuest)
        {
            throw new NotImplementedException();
        }

        public void playerLeft(string playerNmae)
        {
            throw new NotImplementedException();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (isConnected)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (_lobbyClient.State == CommunicationState.Opened)
                        {
                            await _lobbyClient.LeaveLobbyAsync();
                            _lobbyClient.Close();
                        }
                    }
                    catch
                    {
                        _lobbyClient?.Abort();
                    }
                });
            }
            else
            {
                _lobbyClient?.Abort();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Window owner = this.Owner;
            if (owner != null)
            {
                owner.Show();
            }
            if (isConnected)
            {
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    try
                    {
                        if (_lobbyClient.State == CommunicationState.Opened)
                        {
                            await _lobbyClient.LeaveLobbyAsync();
                            _lobbyClient.Close();
                        }
                    }
                    catch
                    {
                        _lobbyClient?.Abort();
                    }
                });
            }
            else
            {
                _lobbyClient?.Abort();
            }

            base.OnClosed(e);
        }
    }

}

