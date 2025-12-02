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

namespace Client.Views.Multiplayer
{
    /// <summary>
    /// Lógica de interacción para PlayGameMultiplayer.xaml
    /// </summary>
    public partial class PlayGameMultiplayer : Window
    {
        public PlayGameMultiplayer(List<GameLobbyServiceReference.CardInfo> cards)
        {
            InitializeComponent();
        }

        private void ButtonSendMessageChat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            Window hostLobby = this.Owner;

            if (hostLobby != null)
            {
                hostLobby.WindowState = this.WindowState;
                hostLobby.Show();
            }
            this.Close();

        }
        
    }
}
