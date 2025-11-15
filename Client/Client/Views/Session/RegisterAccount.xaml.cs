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
    /// Lógica de interacción para RegisterAccount.xaml
    /// </summary>
    public partial class RegisterAccount : Window
    {
        private readonly UserServiceReference.UserServiceClient _userServiceClient = new UserServiceReference.UserServiceClient();
        public RegisterAccount()
        {
            InitializeComponent();
        }

        private async void ButtonAcceptRegisterAccount_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmail.Text.Trim();
            string password = TextBoxPassword.Text?.Trim();

            LabelEmailError.Content = "";
            LabelPasswordError.Content = "";

            ValidationCode validationEmail = Helpers.ValidationHelper.ValidateEmail(email);
            if (validationEmail != ValidationCode.Success)
            {
                LabelEmailError.Content = Helpers.LocalizationManager.GetString(validationEmail);
                return;
            }

            ValidationCode validationPassword = Helpers.ValidationHelper.ValidatePassword(password);
            if (validationPassword != ValidationCode.Success)
            {
                LabelPasswordError.Content = Helpers.LocalizationManager.GetString(validationPassword);
                return;
            }

            ButtonAcceptRegisterAccount.IsEnabled = false;

            try
            {
                ResponseDTO response = await _userServiceClient.StartRegistrationAsync(email, password);

                if (response.Success)
                {
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Success, 
                        Lang.RegisterAccount_Message_Success, 
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Success);
                    msgBox.ShowDialog();

                    var verifyCodeWindow = new VerifyCode(email);
                    verifyCodeWindow.WindowState = this.WindowState;
                    verifyCodeWindow.Owner = this;
                    verifyCodeWindow.Show();
                    this.Hide();
                }
                else
                {
                    string errorMessage = GetServerErrorMessage(response.MessageKey);
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptRegisterAccount.IsEnabled = true;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_ServerOffline, errorMessage, 
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptRegisterAccount.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage, 
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptRegisterAccount.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage, 
                    this, Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptRegisterAccount.IsEnabled = true;
            }
        }

        private string GetServerErrorMessage(string messageKey)
        {
            switch (messageKey)
            {
                case "Global_Error_PasswordInvalid":
                    return Lang.Global_Error_PasswordInvalid;
                case "Global_Error_EmailInUse":
                    return Lang.Global_Error_EmailInUse;
                case "Global_Error_EmailSendFailed":
                    return Lang.Global_Error_EmailSendFailed;
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
                if (_userServiceClient.State != System.ServiceModel.CommunicationState.Faulted)
                {
                    _userServiceClient.Close();
                }
                else
                {
                    _userServiceClient.Abort();
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
