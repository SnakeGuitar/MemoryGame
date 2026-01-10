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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public HostLobby()
        {
            InitializeComponent();

            _lobbyCode = ClientHelper.GenerateGameCode();

            if (LabelGameCode != null)
            {
                LabelGameCode.Content = _lobbyCode;
            }
            if (LabelTimerValue != null)
            {
                LabelTimerValue.Content = _secondsPerTurn.ToString();
            }
            if (ComboBoxNumberOfCards != null)
            {
                ComboBoxNumberOfCards.SelectedIndex = 0;
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

        #region Loading & Creation

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = await GameServiceManager.Instance.Client.CreateLobbyAsync(
                    UserSession.SessionToken, _lobbyCode);

                if (success)
                {
                    _isConnected = true;
                    string myName = UserSession.Username;

                    if (!_currentPlayers.Any(p => p.Name == myName))
                    {
                        _currentPlayers.Add(new LobbyPlayerInfo { Name = myName });
                        UpdatePlayerUI();
                    }

                    new CustomMessageBox(Lang.Global_Title_Success, Lang.Lobby_Message_CreateSuccess, this, MessageBoxType.Success).ShowDialog();
                }
                else
                {
                    new CustomMessageBox(Lang.Global_Title_Error, Lang.HostLobby_Error_CreateFailed, this, MessageBoxType.Error).ShowDialog();
                    GoBackToMenu();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, async () =>
                {
                    await LeaveLobbySafe();
                    GoBackToMenu();
                });
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

            PlayersListBox.Items.Clear();
            foreach (var player in _currentPlayers)
            {
                string displayName = player.Name == UserSession.Username ? $"{player.Name} (Host)" : player.Name;
                PlayersListBox.Items.Add(displayName);
            }
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

        private void ButtonStartMatch_Click(object sender, RoutedEventArgs e)
        {
            if (!_isConnected)
            {
                return;
            }

            if (_currentPlayers.Count < GameConstants.MinPlayersToPlay)
            {
                MessageBox.Show("Waiting for more players to join...", "Cannot Start", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var settings = new GameSettings
                {
                    CardCount = _selectedCardCount,
                    TurnTimeSeconds = _secondsPerTurn
                };

                if (ButtonStart != null)
                {
                    ButtonStart.IsEnabled = false;
                }

                GameServiceManager.Instance.Client.StartGame(settings);
            }
            catch (Exception ex)
            {
                if (ButtonStart != null)
                {
                    ButtonStart.IsEnabled = true;
                }
                ExceptionManager.Handle(ex, this);
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
                    _currentPlayers.Remove(playerToRemove);
                    UpdatePlayerUI();
                    OnChatMessageReceived("System", $"{playerName} left the lobby.", true);
                }
            });
        }

        private async void ButtonSendMessageToChat_Click(object sender, RoutedEventArgs e)
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
                    Debug.WriteLine($"Chat Error: {ex.Message}");
                }
            }
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
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new MultiplayerMenu());
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