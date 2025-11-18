using Client.GameLobbyServiceReference;
using Client.Helpers;
using Client.Views.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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

        private void ButtonAcceptCode_Click(object sender, RoutedEventArgs e)
        {
            string lobbyCode = TextBoxLobbyCode.Text?.Trim();
            LabelCodeError.Content = "";

            ValidationCode validationCode = Helpers.ValidationHelper.ValidateVerifyCode(lobbyCode, GAME_CODE_LENGTH);

            if (validationCode != ValidationCode.Success)
            {
                string errorMessage = Helpers.LocalizationManager.GetString(validationCode);
                LabelCodeError.Content = errorMessage;
                return;
            }

            var lobbyWindow = new Lobby.Lobby(lobbyCode);
            lobbyWindow.WindowState = this.WindowState;
            lobbyWindow.Owner = this;
            lobbyWindow.Show();
            this.Hide();


        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Window mainMenu = this.Owner;

            if (mainMenu != null)
            {
                mainMenu.WindowState = this.WindowState;
                mainMenu.Show();
            }
            this.Close();
        }
    }
}
