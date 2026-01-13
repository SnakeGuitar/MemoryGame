using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static Client.Helpers.LocalizationHelper;
using static Client.Helpers.ValidationHelper;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Multiplayer
{
    /// <summary>
    /// Lógica de interacción para JoinLobby.xaml
    /// </summary>
    public partial class JoinLobby : Window
    {
        private const int GAME_CODE_LENGTH = 6;
        public JoinLobby()
        {
            InitializeComponent();
            LoadPublicMatches();
        }

        private async void LoadPublicMatches()
        {
            ButtonRefresh.IsEnabled = false;


            await ExceptionManager.ExecuteSafeAsync(async () =>
            {
                var lobbies = await GameServiceManager.Instance.GetPublicLobbiesAsync();
                ListBoxPublicLobbies.ItemsSource = lobbies;
            });

            ButtonRefresh.IsEnabled = true;
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadPublicMatches();
        }

        private void ListBoxPublicLobbies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxPublicLobbies.SelectedItem is LobbySummaryDTO selectedLobby)
            {
                if (selectedLobby.IsFull)
                {
                    new CustomMessageBox(Lang.Global_Title_Information, Lang.Lobby_Error_JoinFailed, this, MessageBoxType.Information).ShowDialog();
                    return;
                }

                TextBoxLobbyCode.Text = selectedLobby.GameCode;
                ButtonAcceptCode_Click(sender, e);
            }
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+", RegexOptions.None, TimeSpan.FromMilliseconds(100));
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void ButtonAcceptCode_Click(object sender, RoutedEventArgs e)
        {
            string lobbyCode = TextBoxLobbyCode.Text?.Trim();
            LabelCodeError.Content = "";

            ValidationCode validationCode = ValidateVerifyCode(lobbyCode, GAME_CODE_LENGTH);

            if (validationCode != ValidationCode.Success)
            {
                LabelCodeError.Content = GetString(validationCode);
                return;
            }

            var lobbyWindow = new Lobby.Lobby(lobbyCode);
            NavigationHelper.NavigateTo(this, lobbyWindow);
        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new MultiplayerMenu());
        }
    }
}