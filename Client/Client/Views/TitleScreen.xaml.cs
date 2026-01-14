using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using Client.Views.Session;
using System.Windows;

namespace Client.Views
{
    /// <summary>
    /// Lógica de interacción para TitleScreen.xaml
    /// </summary>
    public partial class TitleScreen : Window
    {
        public TitleScreen()
        {
            InitializeComponent();
        }

        private void ButtonLoginAsGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new EnterUsernameGuest());
        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new Login());
        }
        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new RegisterAccount());
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new Settings(false, false);
            NavigationHelper.NavigateTo(this, settingsWindow);
        }
        private void ButtonExitGame_Click(object sender, RoutedEventArgs e)
        {
            var confirmationBox = new ConfirmationMessageBox(
                Lang.Global_Title_ExitGame, Lang.Global_Message_ExitGame,
                this, ConfirmationMessageBox.ConfirmationBoxType.Critic);

            if (confirmationBox.ShowDialog() == true)
            {
                _ = NavigationHelper.ExitApplication();
            }
        }
    }
}