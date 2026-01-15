using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Models;
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
    public partial class HostLobby : Window
    {
        #region Private Fields

        private readonly string _lobbyCode;
        private bool _isConnected = false;
        private bool _isGameStarting = false;
        private List<LobbyPlayerInfo> _currentPlayers = new List<LobbyPlayerInfo>();
        private int _selectedCardCount = GameConstants.DefaultCardCount;
        private int _secondsPerTurn = GameConstants.DefaultTurnTimeSeconds;

        #endregion

        #region Public Properties

        public bool IsLobbyPreRegistered { get; set; } = false;
        public string LobbyCode => _lobbyCode;

        #endregion

        public HostLobby()
        {
            InitializeComponent();

            _lobbyCode = ClientHelper.GenerateGameCode();

            if (LabelGameCode != null)
            {
                LabelGameCode.Content = _lobbyCode;
            }

            ConfigureEvents();

            this.Loaded += Window_Loaded;
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

        #region Loading & Creation

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool createdSuccessfully = await ExceptionManager.ExecuteNetworkCallAsync(async () =>
            {
                var sessionCheck = await UserServiceManager.Instance.RenewSessionAsync(UserSession.SessionToken);
                if (!sessionCheck.Success)
                {
                    throw new FaultException(sessionCheck.MessageKey);
                }

                if (IsLobbyPreRegistered)
                {
                    Debug.WriteLine("[HostLobby] Pre-registered.");
                }
                else
                {
                    bool result = await GameServiceManager.Instance.CreateLobbyAsync(
                        UserSession.SessionToken, _lobbyCode, false);

                    if (!result)
                    {
                        throw new Exception(Lang.HostLobby_Error_CreateFailed);
                    }
                }
            }, this);

            if (createdSuccessfully)
            {
                _isConnected = true;
                string myName = UserSession.Username;
                if (!_currentPlayers.Any(p => p.Name == myName))
                {
                    _currentPlayers.Add(new LobbyPlayerInfo { Name = myName });
                    UpdatePlayerUI();
                }

                if (!IsLobbyPreRegistered)
                {
                    new CustomMessageBox(Lang.Global_Title_Success, Lang.Lobby_Message_CreateSuccess, this, MessageBoxType.Success).ShowDialog();
                }
            }
            else
            {
                GoBackToMenu();
            }
        }

        #endregion

        #region UI Updates (Thread Safe)

        private void OnPlayerListUpdated(LobbyPlayerInfo[] players)
        {
            if (_isGameStarting)
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                if (_isGameStarting || !this.IsLoaded)
                {
                    return;
                }

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

            PlayersListBox.ItemsSource = _currentPlayers
                .Select(player => player.Name == UserSession.Username ? $"{player.Name} (Host)" : player.Name)
                .ToList();
        }

        private void OnChatMessageReceived(string sender, string message, bool isNotification)
        {
            if (_isGameStarting)
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                if (_isGameStarting || !this.IsLoaded)
                {
                    return;
                }

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

        #region Game Logic

        private async void ButtonStartMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!_isConnected)
            {
                return;
            }

            if (_currentPlayers.Count < GameConstants.MinPlayersToPlay)
            {
                new CustomMessageBox(Lang.Global_Label_CannotStart, Lang.Lobby_Message_WaitingPlayers, this, MessageBoxType.Warning).ShowDialog();
                return;
            }

            if (ButtonStart != null)
            {
                ButtonStart.IsEnabled = false;
            }

            bool started = await ExceptionManager.ExecuteNetworkCallAsync(async () =>
            {
                var sessionCheck = await UserServiceManager.Instance.RenewSessionAsync(UserSession.SessionToken);
                if (!sessionCheck.Success)
                {
                    throw new FaultException(sessionCheck.MessageKey);
                }

                var settings = new GameSettings
                {
                    CardCount = _selectedCardCount,
                    TurnTimeSeconds = _secondsPerTurn
                };

                GameServiceManager.Instance.StartGameSafe(settings);
                await Task.CompletedTask;
            }, this);

            if (!started)
            {
                if (ButtonStart != null)
                {
                    ButtonStart.IsEnabled = true;
                }
            }
        }

        private void OnGameStarted(List<CardInfo> cards)
        {
            Dispatcher.Invoke(() =>
            {
                _isGameStarting = true;
                UnsubscribeEvents();
                var multiplayerGame = new PlayGameMultiplayer(cards, _currentPlayers);

                if (this.Owner != null)
                {
                    multiplayerGame.Owner = this.Owner;
                }

                NavigationHelper.NavigateTo(this, multiplayerGame);
            });
        }

        #endregion

        #region Interaction & Server Callbacks

        private void OnPlayerLeft(string playerName)
        {
            if (_isGameStarting)
            {
                return;
            }

            Dispatcher.Invoke(() =>
            {
                if (_isGameStarting || !this.IsLoaded)
                {
                    return;
                }

                var playerToRemove = _currentPlayers.FirstOrDefault(p => p.Name == playerName);
                if (playerToRemove != null)
                {
                    string message = string.Format(playerName, Lang.Lobby_Notification_PlayerLeft);
                    _currentPlayers.Remove(playerToRemove);
                    UpdatePlayerUI();
                    OnChatMessageReceived(Lang.Global_Label_System, message, true);
                }
            });
        }

        private async void ButtonSendMessageToChat_Click(object sender, RoutedEventArgs e)
        {
            if (ChatTextBox != null && !string.IsNullOrWhiteSpace(ChatTextBox.Text))
            {
                string message = ChatTextBox.Text;
                ChatTextBox.Text = string.Empty;

                await ExceptionManager.ExecuteNetworkCallAsync(async () =>
                {
                    await GameServiceManager.Instance.SendChatMessageAsync(message);
                }, this);
            }
        }

        private void ButtonInvite_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.ShowDialog(this, new InviteFriendDialog(_lobbyCode));
        }

        private async void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_LeaveLobby, Lang.HostLobby_Message_LeaveLobby,
                this, ConfirmationMessageBox.ConfirmationBoxType.Warning);

            if (confirmationBox.ShowDialog() == true)
            {
                await LeaveLobbySafe();
                GoBackToMenu();
            }
        }

        private void GoBackToMenu()
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new MultiplayerMenu());
        }

        #endregion

        #region Settings UI

        private void ComboBoxNumberOfCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxNumberOfCards.SelectedItem is ComboBoxItem item &&
                int.TryParse(item.Content.ToString(), out int value))
            {
                _selectedCardCount = value;
            }
        }

        private void SliderSecondsPerTurn_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _secondsPerTurn = (int)e.NewValue;
            if (LabelTimerValue != null)
            {
                LabelTimerValue.Content = _secondsPerTurn.ToString();
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

                if (this.Owner != null && Application.Current.MainWindow != this.Owner)
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