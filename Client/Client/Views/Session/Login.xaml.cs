using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Core;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
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

                    string successMessage = string.Format(Lang.Global_Message_Welcome, response.User.Username);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_LoginSuccess, successMessage,
                        this, MessageBoxType.Success);
                    msgBox.ShowDialog();

                    var mainMenu = new MainMenu();
                    mainMenu.WindowState = this.WindowState;
                    Application.Current.MainWindow = mainMenu;

                    mainMenu.Show();

                    this.Close();

                    if (this.Owner != null)
                    {
                        this.Owner.Close();
                    }
                }
                else
                {
                    string errorMessage;

                    if (response.MessageKey == ServerKeys.UserAlreadyLoggedIn)
                    {
                        errorMessage = Lang.Global_Error_InvalidCredentials;
                    }
                    else
                    {
                        errorMessage = GetString(response.MessageKey);
                    }

                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_LoginFailed, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonAcceptLogin.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => ButtonAcceptLogin.IsEnabled = false);
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
            base.OnClosed(e);
        }
    }
}
