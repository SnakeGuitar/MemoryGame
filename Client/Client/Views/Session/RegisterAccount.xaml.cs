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

            try
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

                if (response.Success)
                {
                    new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.RegisterAccount_Message_Success,
                        this, MessageBoxType.Success).ShowDialog();

                    if (string.IsNullOrEmpty(UserSession.SessionToken))
                    {
                        return;
                    }

                    NavigationHelper.NavigateTo(this, new VerifyCode(email, _isGuestRegister));
                }
                else
                {
                    string errorMessage = GetString(response.MessageKey);

                    new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error).ShowDialog();

                    ButtonAcceptRegisterAccount.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => ButtonAcceptRegisterAccount.IsEnabled = true);
            }
        }

        private void ButtonBackToTitleScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new TitleScreen());
        }
    }
}