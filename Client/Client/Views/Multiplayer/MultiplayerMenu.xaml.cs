using Client.Core;
using Client.Properties.Langs;
using Client.Views.Controls;
using Client.Views.Lobby;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private void ButtonCreateLobby_Click(object sender, RoutedEventArgs e)
        {
            if(UserSession.IsGuest)
            {
                string message = Lang.Global_Error_GuestsNotAllowed;
                string title = Lang.Global_Title_NotAvailableFunction;
                var msgBox = new CustomMessageBox(
                    title, message, 
                    this, MessageBoxType.Warning);
                msgBox.ShowDialog();
                return;
            }

            var createHostLobby = new HostLobby();
            createHostLobby.WindowState = this.WindowState;
            createHostLobby.Owner = this;
            createHostLobby.Show();
            this.Hide();


        }

        private void ButtonJoinLobby_Click(object sender, RoutedEventArgs e)
        {
            var joinLobby = new JoinLobby();
            joinLobby.WindowState = this.WindowState;
            joinLobby.Owner = this;
            joinLobby.Show();
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

        protected override void OnClosed(EventArgs e)
        {
            Window owner = this.Owner;
            if (owner != null)
            {
                owner.Show();
            }

            base.OnClosed(e);
        }
    }
}
