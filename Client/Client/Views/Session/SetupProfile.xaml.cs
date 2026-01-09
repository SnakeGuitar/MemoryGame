using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Utilities;
using Client.Views.Controls;
using Microsoft.Win32;
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
            string registeredEmail = string.Format(Lang.Global_Label_RegisteredEmail, email);
            InitializeComponent();
            _email = email ?? throw new ArgumentNullException(nameof(email));
            LabelEmail.Content = registeredEmail;
        }

        private void ButtonSelectAvatar_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = Lang.SetupProfile_Dialog_Title,
                Filter = Lang.SetupProfile_Dialog_Filter,
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    byte[] originalBytes = System.IO.File.ReadAllBytes(dialog.FileName);
                    profileImage = ImageHelper.ResizeImage(originalBytes, 200, 200);

                    var bitmapImage = ImageHelper.ByteArrayToImageSource(profileImage);
                    ProfilePicture.Source = bitmapImage;

                }
                catch (Exception ex)
                {
                    string errorMessage = string.Format(Lang.SetupProfile_Error_ImageLoad, ex.Message);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

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

            try
            {
                LoginResponse response = await UserServiceManager.Instance.Client.FinalizeRegistrationAsync(
                    _email,
                    username,
                    profileImage);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User);

                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.SetupProfile_Message_Success,
                        this, MessageBoxType.Information);
                    msgBox.ShowDialog();

                    var mainMenu = new MainMenu();
                    mainMenu.WindowState = this.WindowState;
                    mainMenu.Show();
                    this.Close();

                    if (this.Owner != null)
                    {
                        this.Owner.Close();
                    }
                }
                else
                {
                    string errorMessage = GetString(response.MessageKey);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptSetupProfile.IsEnabled = true;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_ServerOffline, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptSetupProfile.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptSetupProfile.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptSetupProfile.IsEnabled = true;
            }
        }

        private void ButtonBackToTitleScreen_Click(object sender, RoutedEventArgs e)
        {
            Window titleScreen = this.Owner;

            if (titleScreen != null)
            {
                titleScreen.WindowState = this.WindowState;
                titleScreen.Show();
            }
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}
