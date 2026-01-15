using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using Client.Views.Lobby;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Automation.Peers;
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
                checkBoxIsPublic.IsEnabled = false;
                checkBoxIsPublic.Opacity = 0.5;
                TextBlockTitle.Visibility = Visibility.Collapsed;
                TextBlockSubtitle.Text = Lang.Global_Error_GuestsNotAllowed;
            }
        }

        private async void ButtonCreateLobby_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.IsGuest)
            {
                return;
            }

            ButtonCreateLobby.IsEnabled = false;
            var hostLobby = new HostLobby();
            string generatedCode = hostLobby.LabelGameCode.Content?.ToString();
            bool isPublic = checkBoxIsPublic.IsChecked == true;

            bool success = await ExceptionManager.ExecuteNetworkCallAsync(async () =>
            {
                var sessionCheck = await UserServiceManager.Instance.RenewSessionAsync(UserSession.SessionToken);
                if (!sessionCheck.Success)
                {
                    throw new FaultException(sessionCheck.MessageKey);
                }

                bool result = await GameServiceManager.Instance.CreateLobbyAsync(
                    UserSession.SessionToken,
                    generatedCode,
                    isPublic);

                if (!result)
                {
                    throw new Exception(Lang.HostLobby_Error_CreateFailed);
                }
            }, this);

            if (success)
            {
                hostLobby.IsLobbyPreRegistered = true;
                NavigationHelper.NavigateTo(this, hostLobby);
            }
            else
            {
                hostLobby.Close();
                ButtonCreateLobby.IsEnabled = true;
            }
        }

        private void ButtonJoinLobby_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new JoinLobby());
        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new MainMenu());
        }
    }
}