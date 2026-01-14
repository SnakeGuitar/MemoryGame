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
                new CustomMessageBox(
                    Lang.Global_Title_NotAvailableFunction,
                    Lang.Global_Error_GuestsNotAllowed,
                    this, MessageBoxType.Warning).ShowDialog();
                return;
            }

            ButtonCreateLobby.IsEnabled = false;

            var hostLobby = new HostLobby();
            string generatedCode = hostLobby.LabelGameCode.Content?.ToString();
            bool isPublic = checkBoxIsPublic.IsChecked == true;

            if (string.IsNullOrEmpty(generatedCode))
            {
                hostLobby.Close();
                new CustomMessageBox(Lang.Global_Title_Error, Lang.Lobby_Error_CodeGenerationFailed, this, MessageBoxType.Error).ShowDialog();
                ButtonCreateLobby.IsEnabled = true;
                return;
            }

            bool created = await ExceptionManager.ExecuteSafeAsync(async () =>
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
            });

            if (created)
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