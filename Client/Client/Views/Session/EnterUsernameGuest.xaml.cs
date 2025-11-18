using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
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
using static Client.Helpers.ValidationHelper;

namespace Client.Views.Session
{
    /// <summary>
    /// Lógica de interacción para ChooseUsernameGuest.xaml
    /// </summary>
    public partial class EnterUsernameGuest : Window
    {
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        public EnterUsernameGuest()
        {
            InitializeComponent();
        }

        private async void ButtonAcceptUsernameGuest_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxUsername.Text.Trim();
            LabelUsernameError.Content = "";

            ValidationCode validationUsername = Helpers.ValidationHelper.ValidateUsername(username);

            if (validationUsername != ValidationCode.Success)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(validationUsername);
                LabelUsernameError.Content = errorMessage;
                return;
            }

            ButtonAcceptUsernameGuest.IsEnabled = false;

            try
            {
                LoginResponse response = await _userServiceClient.LoginAsGuestAsync(username);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User.Username);

                    string successMessage = string.Format(Lang.Global_Message_Welcome, response.User.Username);
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_LoginAsGuestSuccess, successMessage, 
                        this, Controls.CustomMessageBox.MessageBoxType.Success);
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
                    string errorMessage = GetServerErrorMessage(response.MessageKey);
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage, 
                        this, Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptUsernameGuest.IsEnabled = true;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_ServerOffline,
                    errorMessage, this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptUsernameGuest.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError,
                    errorMessage, this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptUsernameGuest.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError,
                    errorMessage, this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptUsernameGuest.IsEnabled = true;
            }
        }

        private string GetServerErrorMessage(string messageKey)
        {
            switch (messageKey)
            {
                case "Global_Error_GuestUsernameInvalid":
                    return Lang.Global_Error_InvalidUsername;
                case "Global_Error_UsernameInUse":
                    return Lang.Global_Error_UsernameInUse;
                default:
                    return Lang.Global_ServiceError_Unknown;
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
    }
}
