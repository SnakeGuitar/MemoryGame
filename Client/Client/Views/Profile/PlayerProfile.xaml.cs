using Client.Core;
using Client.Core.Exceptions;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using System;
using System.Threading.Tasks;
using System.Windows;

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
            LoadData();
            _ = LoadCurrentAvatar();
            UserSession.ProfileUpdated += LoadData;
        }

        private async Task LoadCurrentAvatar()
        {
            try
            {
                var bytes = await UserServiceManager.Instance.GetUserAvatarAsync(UserSession.Email);
                if (bytes != null && bytes.Length > 0)
                {
                    ImageAvatar.Source = ImageHelper.ByteArrayToImageSource(bytes);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
        }

        private void LoadData()
        {
            string fullName = $"{UserSession.Name} {UserSession.LastName}".Trim();
            string formatDate = "MMMM dd yyyy";

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
            TextDate.Text = UserSession.RegistrationDate.ToString(formatDate);
            ItemsControlSocialNetworks.ItemsSource = UserSession.SocialNetworks;
        }

        private void ButtonEditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new EditProfile());
        }

        private void ButtonCloseSession_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Button_LogOut, Lang.Global_Message_CloseSession,
                this, ConfirmationMessageBox.ConfirmationBoxType.Critic);

            if (confirmationBox.ShowDialog() == true)
            {
                try
                {
                    if (UserSession.IsGuest)
                    {
                        UserServiceManager.Instance.Client.LogoutGuestAsync(UserSession.SessionToken);
                    }
                    else
                    {
                        UserServiceManager.Instance.Client.LogoutAsync(UserSession.SessionToken);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Logout Error]: {ex}");
                }

                UserSession.EndSession();
                NavigationHelper.NavigateTo(this, new TitleScreen());
            }
        }

        private void ButtonFriends_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new Social.FriendsMenu());
        }

        private void ButtonStats_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new StatsHistory());
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new MainMenu());
        }

        private void OnProfileUpdated()
        {
            _ = LoadCurrentAvatar();
        }

        protected override void OnClosed(EventArgs e)
        {
            UserSession.ProfileUpdated -= OnProfileUpdated;
            base.OnClosed(e);
        }
    }
}