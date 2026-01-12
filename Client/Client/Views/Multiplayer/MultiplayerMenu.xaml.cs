using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using Client.Views.Lobby;
using System;
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

            try
            {
                var hostLobby = new HostLobby();
                string generatedCode = hostLobby.LabelGameCode.Content?.ToString();

                if (string.IsNullOrEmpty(generatedCode))
                {
                    new CustomMessageBox(Lang.Global_Title_Error, Lang.Lobby_Error_CodeGenerationFailed, this, MessageBoxType.Error).ShowDialog();
                    return;
                }

                bool isPublic = checkBoxIsPublic.IsChecked == true;
                bool created = await GameServiceManager.Instance.CreateLobbyAsync(UserSession.SessionToken, generatedCode, isPublic);

                if (created)
                {
                    hostLobby.IsLobbyPreRegistered = true;
                    NavigationHelper.NavigateTo(this, hostLobby);
                }
                else
                {
                    new CustomMessageBox(Lang.Global_Title_Error, Lang.HostLobby_Error_CreateFailed, this, MessageBoxType.Error).ShowDialog();
                    hostLobby.Close();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = LocalizationHelper.GetString(ex);

                new CustomMessageBox(
                    Lang.Global_Title_LoginFailed, errorMessage,
                    this, MessageBoxType.Error).ShowDialog();
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