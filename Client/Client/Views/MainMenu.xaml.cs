using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using Client.Views.Multiplayer;
using Client.Views.Session;
using Client.Views.Singleplayer;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private DispatcherTimer _keepAliveTimer;
        private const int KEEP_ALIVE_INTERVAL_SECONDS = 180;

        public MainMenu()
        {
            InitializeComponent();

            UsernameDisplay.Content = UserSession.Username;

            if (UserSession.IsGuest)
            {
                ButtonSignIn.Visibility = Visibility.Visible;
                ProfilePicture.Source = null;
            }
            else
            {
                ButtonSignIn.Visibility = Visibility.Collapsed;
                UserSession.ProfileUpdated += OnProfileUpdated;
                _ = LoadAvatarAsync();
                InitializeKeepAlive();
            }
        }

        #region Navigation Events

        private void ButtonSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new SelectDifficulty());
        }

        private void ButtonMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new MultiplayerMenu());
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new Settings());
        }

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.IsGuest)
            {
                string message = string.Format(Lang.PlayerProfile_Message_NotAvaible, UserSession.Username);
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NotAvailableFunction, message,
                    this, MessageBoxType.Warning);
                msgBox.ShowDialog();
                return;
            }

            NavigationHelper.NavigateTo(this, new Profile.PlayerProfile());
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new RegisterAccount(isGuestRegister: true));
        }

        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_ExitGame, Lang.Global_Message_ExitGame,
                this, ConfirmationMessageBox.ConfirmationBoxType.Critic);

            if (confirmationBox.ShowDialog() == true)
            {
                PerformLogout();
                NavigationHelper.ExitApplication();
            }
        }

        #endregion

        #region Data & Logic

        private async Task LoadAvatarAsync()
        {
            try
            {
                byte[] avatarBytes = await UserServiceManager.Instance.Client.GetUserAvatarAsync(UserSession.Email);

                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    ProfilePicture.Source = ImageHelper.ByteArrayToImageSource(avatarBytes);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MainMenu] Failed to load avatar: {ex.Message}");
            }
        }

        private void OnProfileUpdated()
        {
            UsernameDisplay.Content = UserSession.Username;
            _ = LoadAvatarAsync();
        }

        private static void PerformLogout()
        {
            if (UserSession.IsGuest)
            {
                try
                {
                    UserServiceManager.Instance.Client.LogoutGuestAsync(UserSession.SessionToken);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[MainMenu] Error on LogoutGuest: {ex.Message}");
                }
            }
            else
            {
                try
                {
                    if (UserServiceManager.Instance.Client.State == CommunicationState.Opened)
                    {
                        UserServiceManager.Instance.Client.LogoutAsync(UserSession.SessionToken);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[MainMenu] Error closing session: {ex.Message}");
                }
            }
        }

        #endregion

        #region Keep Alive (Heartbeat)

        private void InitializeKeepAlive()
        {
            _keepAliveTimer = new DispatcherTimer();
            _keepAliveTimer.Interval = TimeSpan.FromSeconds(KEEP_ALIVE_INTERVAL_SECONDS);
            _keepAliveTimer.Tick += async (s, e) => await SendHeartbeatAsync();
            _keepAliveTimer.Start();
        }

        private async Task SendHeartbeatAsync()
        {
            try
            {
                var response = await UserServiceManager.Instance.Client.RenewSessionAsync(UserSession.SessionToken);

                if (!response.Success)
                {
                    Debug.WriteLine("[MainMenu] Heartbeat: Session expired server-side.");
                    HandleSessionExpiration();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MainMenu] Heartbeat failed: {ex.Message}");
            }
        }

        private void HandleSessionExpiration()
        {
            _keepAliveTimer?.Stop();

            new CustomMessageBox(
                Lang.Global_Title_Error,
                Lang.Global_Error_SessionExpired,
                this,
                MessageBoxType.Error).ShowDialog();

            UserSession.EndSession();

            NavigationHelper.NavigateTo(this, new Login());
        }

        #endregion

        #region Window Lifecycle

        protected override void OnClosed(EventArgs e)
        {
            _keepAliveTimer?.Stop();
            UserSession.ProfileUpdated -= OnProfileUpdated;

            if (Application.Current.MainWindow == this)
            {
                PerformLogout();
            }
            base.OnClosed(e);
        }

        #endregion
    }
}