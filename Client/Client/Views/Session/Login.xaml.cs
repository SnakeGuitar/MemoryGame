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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Client.Helpers.ValidationHelper;

namespace Client.Views.Session
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly UserServiceReference.UserServiceClient _userServiceClient = new UserServiceReference.UserServiceClient();
        public Login()
        {
            InitializeComponent();
        }

        private async void ButtonAcceptLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmailInput.Text?.Trim();
            string password = PasswordBoxPasswordInput.Password;

            LabelEmailError.Content = "";
            LabelPasswordError.Content = "";

            ValidationCode validationEmail = Helpers.ValidationHelper.ValidateEmail(email);
            if (validationEmail != ValidationCode.Success)
            {
                LabelEmailError.Content = Helpers.LocalizationHelper.GetString(validationEmail);
                return;
            }

            ValidationCode validationPassword = Helpers.ValidationHelper.ValidatePassword(password);
            if (validationPassword != ValidationCode.Success)
            {
                LabelPasswordError.Content = Helpers.LocalizationHelper.GetString(validationPassword);
                return;
            }

            ButtonAcceptLogin.IsEnabled = false;

            try
            {
                LoginResponse response = await _userServiceClient.LoginAsync(email, password);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User.UserId, response.User.Username, response.User.Email);

                    string successMessage = string.Format(Lang.Global_Message_Welcome, response.User.Username);
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_LoginSuccess, successMessage, 
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Success);
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
                        Lang.Global_Title_LoginFailed, errorMessage,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptLogin.IsEnabled = true;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_ServerOffline, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptLogin.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptLogin.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptLogin.IsEnabled = true;
            }
        }

        private string GetServerErrorMessage(string messageKey)
        {
            switch (messageKey)
            {
                case "Global_Error_InvalidCredentials":
                    return Lang.Global_Error_InvalidCredentials;
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

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                if (_userServiceClient?.State == System.ServiceModel.CommunicationState.Opened)
                {
                    _userServiceClient.Close();
                }
            }
            catch
            {
                _userServiceClient.Abort();
            }
            base.OnClosed(e);
        }
    }
}
