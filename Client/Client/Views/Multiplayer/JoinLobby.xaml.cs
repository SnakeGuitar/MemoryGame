using Client.Helpers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static Client.Helpers.LocalizationHelper;
using static Client.Helpers.ValidationHelper;

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
        }

        private void NumericOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
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