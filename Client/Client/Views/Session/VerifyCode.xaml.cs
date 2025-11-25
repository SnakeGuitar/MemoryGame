using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
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
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Session
{
    /// <summary>
    /// Lógica de interacción para VerifyCode.xaml
    /// </summary>
    public partial class VerifyCode : Window
    {
        private readonly string _email;
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();
        private const int PIN_LENGTH = 6;
        

        public VerifyCode(string email)
        {
            InitializeComponent();
            _email = email ?? throw new ArgumentNullException(nameof(email));
            LabelRegisterEmail.Content = _email;
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private async void ButtonVerifyCode_Click(object sender, RoutedEventArgs e)
        {
            string code = TextBoxInputVericationCode.Text?.Trim();
            LabelCodeError.Content = "";

            ValidationCode validationCode = Helpers.ValidationHelper.ValidateVerifyCode(code, PIN_LENGTH);
            if (validationCode != ValidationCode.Success)
            {
                LabelCodeError.Content = Helpers.LocalizationHelper.GetString(validationCode);
                return;
            }

            ButtonVerifyCode.IsEnabled = false;

            try
            {
                ResponseDTO response = await _userServiceClient.VerifyRegistrationAsync(_email, code);

                if (response.Success)
                {
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Success,
                        Lang.VerifyCode_Message_Success,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Success);
                    msgBox.ShowDialog();

                    var profileSetupWindow = new SetupProfile(_email);
                    profileSetupWindow.WindowState = this.WindowState;
                    profileSetupWindow.Show();
                    this.Close();
                }
                else
                {
                    string errorMessage = GetServerErrorMessage(response.MessageKey);
                    var msgBox = new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonVerifyCode.IsEnabled = true;
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

                ButtonVerifyCode.IsEnabled = true;
            }
            catch (CommunicationException ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonVerifyCode.IsEnabled = true;
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonVerifyCode.IsEnabled = true;
            }

        }

        private string GetServerErrorMessage(string messageKey)
        {
            switch (messageKey)
            {
                case "Global_Error_CodeInvalid":
                    return Lang.Global_Error_CodeInvalid;
                case "Global_Error_CodeExpired":
                    return Lang.Global_Error_CodeExpired;
                case "Global_Error_RegistrationNotFound":
                    return Lang.Global_Error_RegistrationNotFound;
                case "Global_Error_EmailInUse":
                    return Lang.Global_Error_EmailInUse;
                case "Global_Error_EmailSendFailed":
                    return Lang.Global_Error_EmailSendFailed;
                default:
                    return Lang.Global_ServiceError_Unknown;
            }
        }

        private async void ButtonResendVerificationCode_Click(object sender, RoutedEventArgs e)
        {
            ButtonResendCode.IsEnabled = false;

            try
            {
                ResponseDTO response = await _userServiceClient.ResendVerificationCodeAsync(_email);

                if (response.Success)
                {
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.Verify_Message_CodeResent,
                        this, MessageBoxType.Success);
                    msgBox.ShowDialog();
                }
                else
                {
                    string errorMessage = GetServerErrorMessage(response.MessageKey);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = Helpers.LocalizationHelper.GetString(ex);
                
                Debug.WriteLine($"[Resend Code Error]: {ex.ToString()}");
                var msgBox = new Views.Controls.CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, Views.Controls.CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            finally
            {
                ButtonResendCode.IsEnabled = true;
            }
        }

        private void ButtonBackToSignIn_Click(object sender, RoutedEventArgs e)
        {
            Window registerAccountWindow = this.Owner;

            if (registerAccountWindow != null)
            {
                registerAccountWindow.WindowState = this.WindowState;
                registerAccountWindow.Show();
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
