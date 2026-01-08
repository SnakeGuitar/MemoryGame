using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Utilities;
using Client.Views.Controls;
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

namespace Client.Views.Profile
{
    /// <summary>
    /// Lógica de interacción para PlayerProfile.xaml
    /// </summary>
    public partial class PlayerProfile : Window
    {
        public PlayerProfile()
        {
            InitializeComponent();
            _ = LoadCurrentAvatar();
            LoadData();
            UserSession.ProfileUpdated += LoadData;
        }

        private async Task LoadCurrentAvatar()
        {
            try
            {
                var bytes = await UserServiceManager.Instance.Client.GetUserAvatarAsync(UserSession.Email);
                if (bytes != null && bytes.Length > 0)
                {
                    ImageAvatar.Source = ImageHelper.ByteArrayToImageSource(bytes);
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

        private void LoadData()
        {
            string fullName = $"{UserSession.Name} {UserSession.LastName}".Trim();

            if (string.IsNullOrEmpty(fullName))
            {
                TextBlockFullName.Text = Lang.Global_Label_NoRegister;
            }
            else
            {
                TextBlockFullName.Text = fullName;
            }

            TextBlockUsername.Text = UserSession.Username;
            TextEmail.Text = UserSession.Email;
            TextDate.Text = UserSession.RegistrationDate.ToString("MMMM dd, yyyy");
            ItemsControlSocialNetworks.ItemsSource = UserSession.SocialNetworks;
        }

        private void ButtonEditProfile_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new Profile.EditProfile();
            profileWindow.WindowState = this.WindowState;
            profileWindow.Owner = this;
            profileWindow.Show();
            this.Hide();
        }

        private void ButtonCloseSession_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Button_CloseSession, Lang.Global_Message_CloseSession,
                this, ConfirmationMessageBox.ConfirmationBoxType.Critic);

            bool? result = confirmationBox.ShowDialog();
            if (result == true)
            {
                if (UserSession.IsGuest)
                {
                    try
                    {
                        UserServiceManager.Instance.Client.LogoutGuestAsync(UserSession.SessionToken);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error on LogoutGuest: {ex.Message}");
                    }
                }
                UserSession.EndSession();

                var titleScreen = new TitleScreen();
                titleScreen.WindowState = this.WindowState;
                titleScreen.Show();
                this.Close();
            }
        }

        private void ButtonFriends_Click(object sender, RoutedEventArgs e)
        {
            var friendsWindow = new Social.FriendsMenu();
            friendsWindow.WindowState = this.WindowState;
            friendsWindow.Owner = this;
            friendsWindow.Show();
            this.Hide();
        }

        private void ButtonStats_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new Profile.StatsHistory();
            statsWindow.WindowState = this.WindowState;
            statsWindow.Owner = this;
            statsWindow.Show();
            this.Hide();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Window mainMenu = this.Owner;

            if (mainMenu != null)
            {
                mainMenu.WindowState = this.WindowState;
                mainMenu.Show();
            }
            this.Close();
        }

        private void OnProfileUpdated()
        {
            _ = LoadCurrentAvatar();
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
