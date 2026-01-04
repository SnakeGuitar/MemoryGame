using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
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
using static Client.Helpers.ValidationHelper;
using static Client.Views.Controls.CustomMessageBox;

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

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private async void ButtonAcceptUsernameGuest_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxUsername.Text.Trim();
            LabelUsernameError.Content = "";

            ValidationCode validationUsername = ValidateUsername(username);

            if (validationUsername != ValidationCode.Success)
            {
                string errorMessage = GetString(validationUsername);
                LabelUsernameError.Content = errorMessage;
                return;
            }

            ButtonAcceptUsernameGuest.IsEnabled = false;

            try
            {
                LoginResponse response = await _userServiceClient.LoginAsGuestAsync(username);

                if (response.Success)
                {
                    UserSession.StartGuestSession(response.SessionToken, response.User.UserId, response.User.Username);

                    string successMessage = string.Format(Lang.Global_Message_Welcome, response.User.Username);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_LoginAsGuestSuccess, successMessage, 
                        this, MessageBoxType.Success);
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
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage, 
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptUsernameGuest.IsEnabled = true;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_ServerOffline,
                    errorMessage, this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptUsernameGuest.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError,
                    errorMessage, this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptUsernameGuest.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError,
                    errorMessage, this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptUsernameGuest.IsEnabled = true;
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
