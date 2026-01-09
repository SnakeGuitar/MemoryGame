using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Utilities;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
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
                LoginResponse response = await UserServiceManager.Instance.Client.LoginAsGuestAsync(username);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User);

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
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => ButtonAcceptUsernameGuest.IsEnabled = true);
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
