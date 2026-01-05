using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using Client.Views.Multiplayer;
using Client.Views.Session;
using Client.Views.Singleplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using static Client.Helpers.LocalizationHelper;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        public MainMenu()
        {
            InitializeComponent();
            UsernameDisplay.Content = UserSession.Username;
            ButtonSignIn.Visibility = Visibility.Visible;
            UserSession.ProfileUpdated += OnProfileUpdated;

            if (!UserSession.IsGuest)
            {
                _ = LoadAvatarAsync();
                ButtonSignIn.Visibility = Visibility.Collapsed;
            }

        }

        private void ButtonSinglePlayer_Click(object sender, RoutedEventArgs e)
        {
            var singlePlayerMenu = new SelectDifficulty();
            singlePlayerMenu.WindowState = this.WindowState;
            singlePlayerMenu.Owner = this;
            singlePlayerMenu.Show();
            this.Hide();
        }

        private void ButtonMultiplayer_Click(object sender, RoutedEventArgs e)
        {
            var multiplayerMenu = new MultiplayerMenu();
            multiplayerMenu.WindowState = this.WindowState;
            multiplayerMenu.Owner = this;
            multiplayerMenu.Show();
            this.Hide();
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new Settings();
            settingsWindow.WindowState = this.WindowState;
            settingsWindow.Owner = this;
            settingsWindow.Show();
            this.Hide();

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

            var profileWindow = new Profile.PlayerProfile();
            profileWindow.WindowState = this.WindowState;
            profileWindow.Owner = this;
            profileWindow.Show();
            this.Hide();
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            var signInWindow = new RegisterAccount(true);
            signInWindow.WindowState = this.WindowState;
            signInWindow.Owner = this;
            signInWindow.Show();
            this.Hide();
        }

        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_ExitGame, Lang.Global_Message_ExitGame,
                this, ConfirmationMessageBox.ConfirmationBoxType.Critic);

            bool? result = confirmationBox.ShowDialog();
            if (result == true)
            {
                if (UserSession.IsGuest)
                {
                    try
                    {
                        _userServiceClient.LogoutGuestAsync(UserSession.SessionToken);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error on LogoutGuest: {ex.Message}");
                    }
                }
                UserSession.EndSession();
                Application.Current.Shutdown();
            }
        }

        private async Task LoadAvatarAsync()
        {
            try
            {
                byte[] avatarBytes = await _userServiceClient.GetUserAvatarAsync(UserSession.Email);

                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    ProfilePicture.Source = ImageHelper.ByteArrayToImageSource(avatarBytes);
                }
                else
                {
                    Debug.WriteLine($"Avatar not found for user: {UserSession.Username}");
                }

            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }

        private void OnProfileUpdated()
        {
            UsernameDisplay.Content = UserSession.Username;
            _ = LoadAvatarAsync();
        }
        protected override void OnClosed(EventArgs e)
        {
            UserSession.ProfileUpdated -= OnProfileUpdated;
            Window owner = this.Owner;
            if (owner != null)
            {
                owner.Show();
            }
            base.OnClosed(e);
        }
    }
}
