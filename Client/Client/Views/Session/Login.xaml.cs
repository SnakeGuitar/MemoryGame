using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
using System.Windows;
using System.Windows.Input;
using static Client.Helpers.LocalizationHelper;
using static Client.Helpers.ValidationHelper;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Session
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
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

        private async void ButtonAcceptLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmailInput.Text?.Trim();
            string password = PasswordBoxPasswordInput.Password;

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

            ButtonAcceptLogin.IsEnabled = false;

            try
            {
                LoginResponse response = await UserServiceManager.Instance.LoginAsync(email, password);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User);

                    new CustomMessageBox(
                        Lang.Global_Title_LoginSuccess,
                        string.Format(Lang.Global_Message_Welcome, response.User.Username),
                        this, MessageBoxType.Success).ShowDialog();

                    NavigationHelper.NavigateTo(this, new MainMenu());
                }
                else
                {
                    string errorMessage = (response.MessageKey == ServerKeys.UserAlreadyLoggedIn)
                        ? Lang.Global_Error_InvalidCredentials
                        : GetString(response.MessageKey);

                    new CustomMessageBox(
                        Lang.Global_Title_LoginFailed, errorMessage,
                        this, MessageBoxType.Error).ShowDialog();

                    ButtonAcceptLogin.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => ButtonAcceptLogin.IsEnabled = true);
            }
        }

        private void ButtonBackToTitleScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new TitleScreen());
        }
    }
}