using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Multiplayer;
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

namespace Client.Views
{
    /// <summary>
    /// Lógica de interacción para MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private readonly UserServiceReference.UserServiceClient _userServiceClient = new UserServiceReference.UserServiceClient();
        public MainMenu()
        {
            InitializeComponent();

            UsernameDisplay.Content = UserSession.Username;
            
            if (!UserSession.IsGuest)
            {
                LoadAvatarAsync();
            }

        }

        private void ButtonSingleplayer_Click(object sender, RoutedEventArgs e)
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

        private void ButtonGallery_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
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

        private async void LoadAvatarAsync()
        {
            try
            {
                byte[] avatarBytes = await _userServiceClient.GetUserAvatarAsync(UserSession.Username);

                if (avatarBytes != null && avatarBytes.Length > 0)
                {
                    ProfilePicture.Source = Client.Helpers.ImageHelper.ByteArrayToImageSource(avatarBytes);
                }
                else
                {
                    Debug.WriteLine($"Avatar not found for user: {UserSession.Username}");
                }
                
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, 
                    Helpers.LocalizationManager.GetString(ex),
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            Window owner = this.Owner;
            if (owner != null)
            {
                owner.Show();
            }
            base.OnClosed(e);
        }
    }
}
