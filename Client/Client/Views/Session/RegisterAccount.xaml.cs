using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
using System.Windows;
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

            bool success = await ExceptionManager.ExecuteSafeAsync(async () =>
            {
                ResponseDTO response;

                if (_isGuestRegister)
                {
                    response = await UserServiceManager.Instance.InitiateGuestRegistrationAsync(
                        UserSession.UserId, email, password);
                }
                else
                {
                    response = await UserServiceManager.Instance.StartRegistrationAsync(email, password);
                }

                if (!response.Success)
                {
                    throw new Exception(GetString(response.MessageKey));
                }
            });

            if (success)
            {
                new CustomMessageBox(
                    Lang.Global_Title_Success, Lang.RegisterAccount_Message_Success,
                    this, MessageBoxType.Success).ShowDialog();

                var verifyCode = new VerifyCode(email, _isGuestRegister);
                verifyCode.Owner = this;
                verifyCode.Show();

                this.Hide();
            }
            else
            {
                ButtonAcceptRegisterAccount.IsEnabled = true;
            }
        }

        private void ButtonBackToTitleScreen_Click(object sender, RoutedEventArgs e)
        {
            Window windowToOpen;

            if (_isGuestRegister)
            {
                windowToOpen = new MainMenu();
            }
            else
            {
                windowToOpen = new TitleScreen();
            }

            NavigationHelper.NavigateTo(this, windowToOpen);
        }
    }
}