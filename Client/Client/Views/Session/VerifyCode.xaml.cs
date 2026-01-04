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
using static Client.Helpers.LocalizationHelper;
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
        private readonly bool _isGuestRegister;

        public VerifyCode(string email, bool isGuestRegister = false)
        {
            InitializeComponent();
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _isGuestRegister = isGuestRegister;
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

            ValidationCode validationCode = ValidateVerifyCode(code, PIN_LENGTH);
            if (validationCode != ValidationCode.Success)
            {
                LabelCodeError.Content = GetString(validationCode);
                return;
            }

            ButtonVerifyCode.IsEnabled = false;

            try
            {
                ResponseDTO response;
                string messageSuccess = Lang.VerifyCode_Message_Success;

                if (_isGuestRegister)
                {
                    response = await _userServiceClient.VerifyGuestRegistrationAsync(UserSession.UserId, _email, code);
                    messageSuccess = Lang.VerifyCode_Message_GuestSuccess;
                }
                else
                {
                    response = await _userServiceClient.VerifyRegistrationAsync(_email, code);
                }

                if (response.Success)
                {
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Success, messageSuccess,
                        this, MessageBoxType.Success);
                    msgBox.ShowDialog();

                    if (_isGuestRegister)
                    {
                        UserSession.EndSession();

                        Login loginWindow = new Login();
                        Application.Current.MainWindow = loginWindow;
                        loginWindow.Show();

                        Window ownerWindow = this.Owner;
                        this.Close();
                        ownerWindow?.Close();
                    }
                    else
                    {
                        var profileSetupWindow = new SetupProfile(_email);
                        profileSetupWindow.WindowState = this.WindowState;
                        profileSetupWindow.Show();
                        this.Close();
                    }
                }
                else
                {
                    string errorMessage = LocalizationHelper.GetString(response.MessageKey);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonVerifyCode.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);
                Debug.WriteLine($"[Error]: {ex.Message}");

                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_Error, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonVerifyCode.IsEnabled = true;
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
                    string errorMessage = GetString(response.MessageKey);
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
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, CustomMessageBox.MessageBoxType.Error);
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
                if (_userServiceClient?.State == CommunicationState.Opened)
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
