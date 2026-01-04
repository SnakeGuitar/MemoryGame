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
    /// Lógica de interacción para RegisterAccount.xaml
    /// </summary>
    public partial class RegisterAccount : Window
    {
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        private readonly bool _isGuestRegister;
        public RegisterAccount(bool isGuestRegister = false)
        {
            InitializeComponent();
            _isGuestRegister = isGuestRegister;

            if (_isGuestRegister)
            {
                TitleRegister.Content = Lang.RegisterAccount_Title_LinkAccount;
            }
        }

        private async void ButtonAcceptRegisterAccount_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmail.Text.Trim();
            string password = TextBoxPassword.Text?.Trim();

            LabelEmailError.Content = "";
            LabelPasswordError.Content = "";

            ValidationCode validationEmail = ValidateEmail(email);
            if (validationEmail != ValidationCode.Success)
            {
                LabelEmailError.Content = GetString(validationEmail);
                return;
            }

            ValidationCode validationPassword = ValidatePassword(password);
            if (validationPassword != ValidationCode.Success)
            {
                LabelPasswordError.Content = GetString(validationPassword);
                return;
            }

            ButtonAcceptRegisterAccount.IsEnabled = false;

            try
            {
                ResponseDTO response;

                if (_isGuestRegister)
                {
                    response = await _userServiceClient.InitiateGuestRegistrationAsync(
                        UserSession.UserId, email, password);
                }
                else
                {
                    response = await _userServiceClient.StartRegistrationAsync(email, password);
                }

                if (response.Success)
                {
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.RegisterAccount_Message_Success,
                        this, MessageBoxType.Success);
                    msgBox.ShowDialog();

                    var verifyCodeWindow = new VerifyCode(email, _isGuestRegister);
                    verifyCodeWindow.WindowState = this.WindowState;
                    verifyCodeWindow.Owner = this;
                    verifyCodeWindow.Show();
                    this.Hide();
                }
                else
                {
                    string errorMessage = GetString(response.MessageKey);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptRegisterAccount.IsEnabled = true;
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

                ButtonAcceptRegisterAccount.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage, 
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptRegisterAccount.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage, 
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonAcceptRegisterAccount.IsEnabled = true;
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
