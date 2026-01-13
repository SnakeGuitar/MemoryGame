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
            var settingsWindow = new Settings(false, false);
            NavigationHelper.NavigateTo(this, settingsWindow);
        }

        private void ButtonGallery_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new CardGallery());
        }

        private void ButtonProfile_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.IsGuest)
            {
                string message = string.Format(Lang.PlayerProfile_Message_NotAvaible, UserSession.Username);
                var messageBox = new CustomMessageBox(
                    Lang.Global_Title_NotAvailableFunction, message,
                    this, MessageBoxType.Warning);
                messageBox.ShowDialog();
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
                NavigationHelper.ExitApplication();
            }
        }

        #endregion

        #region Data & Logic

        private async Task LoadAvatarAsync()
        {
            await ExceptionManager.ExecuteSafeAsync(async () =>
            {
                byte[] avatarBytes = await UserServiceManager.Instance.GetUserAvatarAsync(UserSession.Email);

                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    ProfilePicture.Source = ImageHelper.ByteArrayToImageSource(avatarBytes);
                }
            });
        }

        private void OnProfileUpdated()
        {
            UsernameDisplay.Content = UserSession.Username;
            _ = LoadAvatarAsync();
        }

        #endregion

        #region Keep Alive (Heartbeat)

        private void InitializeKeepAlive()
        {
            _keepAliveTimer = new DispatcherTimer();
            _keepAliveTimer.Interval = TimeSpan.FromSeconds(KEEP_ALIVE_INTERVAL_SECONDS);
            _keepAliveTimer.Tick += async (s, e) => await SendHeartbeatSafeAsync();
            _keepAliveTimer.Start();
        }

        private async Task SendHeartbeatSafeAsync()
        {
            bool connectionAlive = await ExceptionManager.ExecuteSafeAsync(async () =>
            {
                var response = await UserServiceManager.Instance.RenewSessionAsync(UserSession.SessionToken);

                if (!response.Success)
                {
                    throw new Exception(Lang.Global_Error_SessionExpired);
                }
            });

            if (!connectionAlive)
            {
                _keepAliveTimer.Stop();
                UserSession.EndSession();
                NavigationHelper.NavigateTo(this, new Login());
            }
        }

        #endregion

        #region Window Lifecycle

        protected override void OnClosed(EventArgs e)
        {
            _keepAliveTimer?.Stop();
            UserSession.ProfileUpdated -= OnProfileUpdated;
            base.OnClosed(e);
        }

        #endregion
    }
}