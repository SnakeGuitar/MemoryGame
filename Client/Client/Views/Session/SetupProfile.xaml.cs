using Client.Core;
using Client.Core.Exceptions;
using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using Microsoft.Win32;
using System;
using System.IO;
using System.ServiceModel;
using System.Windows;
using static Client.Helpers.LocalizationHelper;
using static Client.Helpers.ValidationHelper;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Session
{
    /// <summary>
    /// Lógica de interacción para SetupProfile.xaml
    /// </summary>
    public partial class SetupProfile : Window
    {
        private readonly string _email;
        private byte[] profileImage;

        public SetupProfile(string email)
        {
            InitializeComponent();
            _email = email ?? throw new ArgumentNullException(nameof(email));
            LabelEmail.Content = string.Format(Lang.Global_Label_RegisteredEmail, email);
        }

        private void ButtonSelectAvatar_Click(object sender, RoutedEventArgs e)
        {
            var avatarDialog = NavigationHelper.GetOpenFileDialog(
                Lang.SetupProfile_Dialog_Title, 
                Lang.SetupProfile_Dialog_Filter, 
                false);

            if (avatarDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] originalBytes = File.ReadAllBytes(avatarDialog.FileName);
                    profileImage = ImageHelper.ResizeImage(originalBytes, 200, 200);
                    ProfilePicture.Source = ImageHelper.ByteArrayToImageSource(profileImage);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Handle(ex, this);
                    profileImage = null;
                    ProfilePicture.Source = null;
                }
            }
        }

        private async void ButtonAcceptSetup_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxUsername.Text.Trim();
            LabelUsernameError.Content = "";

            ValidationCode validationCode = ValidateUsername(username);
            if (validationCode != ValidationCode.Success)
            {
                LabelUsernameError.Content = GetString(validationCode);
                return;
            }

            ButtonAcceptSetupProfile.IsEnabled = false;

            bool success = await ExceptionManager.ExecuteNetworkCallAsync(async () =>
            {
                LoginResponse response = await UserServiceManager.Instance.FinalizeRegistrationAsync(
                    _email, username, profileImage);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User);
                }
                else
                {
                    throw new FaultException(response.MessageKey);
                }
            }, this, NetworkFailPolicy.ShowWarningOnly);

            if (success)
            {
                new CustomMessageBox(
                    Lang.Global_Title_Success, Lang.SetupProfile_Message_Success,
                    this, MessageBoxType.Information).ShowDialog();

                NavigationHelper.NavigateTo(this, new MainMenu());
            }
            else
            {
                ButtonAcceptSetupProfile.IsEnabled = true;
            }
        }

        private void ButtonBackToTitleScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new TitleScreen());
        }
    }
}