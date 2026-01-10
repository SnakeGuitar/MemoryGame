using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using Client.Views.Lobby;
using System.Windows;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Multiplayer
{
    /// <summary>
    /// Lógica de interacción para MultiplayerMenu.xaml
    /// </summary>
    public partial class MultiplayerMenu : Window
    {
        public MultiplayerMenu()
        {
            InitializeComponent();

            if (UserSession.IsGuest)
            {
                ButtonCreateLobby.IsEnabled = false;
                ButtonCreateLobby.Opacity = 0.5;
            }
        }

        private void ButtonCreateLobby_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.IsGuest)
            {
                new CustomMessageBox(
                    Lang.Global_Title_NotAvailableFunction,
                    Lang.Global_Error_GuestsNotAllowed,
                    this, MessageBoxType.Warning).ShowDialog();
                return;
            }

            NavigationHelper.NavigateTo(this, new HostLobby());
        }

        private void ButtonJoinLobby_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new JoinLobby());
        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new MainMenu());
        }
    }
}