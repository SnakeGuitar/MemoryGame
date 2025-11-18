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

namespace Client.Views.Singleplayer
{
    /// <summary>
    /// Lógica de interacción para PlayGameSingleplayer.xaml
    /// </summary>
    public partial class PlayGameSingleplayer : Window
    {
        public PlayGameSingleplayer()
        {
            InitializeComponent();
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {

        }

        public void ButtonBackToSelectDifficulty_Click(object sender, RoutedEventArgs e)
        {
            Window menuDifficulty = this.Owner;

            if (menuDifficulty != null)
            {
                menuDifficulty.WindowState = this.WindowState;
                menuDifficulty.Show();
            }
            this.Close();
        }

        
    }
}
