using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
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
                LabelUsernameError.Content = GetString(validationUsername);
                return;
            }

            ButtonAcceptUsernameGuest.IsEnabled = false;

            bool success = await ExceptionManager.ExecuteSafeAsync(async () =>
            {
                LoginResponse response = await UserServiceManager.Instance.LoginAsGuestAsync(username);

                if (response.Success)
                {
                    UserSession.StartSession(response.SessionToken, response.User);
                }
                else
                {
                    throw new FaultException(response.MessageKey);
                }
            });

            if (success)
            {
                new CustomMessageBox(
                    Lang.Global_Title_LoginAsGuestSuccess,
                    string.Format(Lang.Global_Message_Welcome, UserSession.Username),
                    this, MessageBoxType.Success).ShowDialog();

                NavigationHelper.NavigateTo(this, new MainMenu());
            }
            else
            {
                ButtonAcceptUsernameGuest.IsEnabled = true;
            }
        }

        private void ButtonBackToTitleScreen_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new TitleScreen());
        }
    }
}