using Client.Models;
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
    /// Lógica de interacción para SelectDifficulty.xaml
    /// </summary>
    public partial class SelectDifficulty : Window
    {
        public SelectDifficulty()
        {
            InitializeComponent();
        }

        private void ButtonEasyDifficulty_Click(object sender, RoutedEventArgs e)
        {
            var playGameSingleplayer = new PlayGameSingleplayer(DifficultyPresets.Easy);
            playGameSingleplayer.WindowState = this.WindowState;
            playGameSingleplayer.Owner = this;
            playGameSingleplayer.Show();
            this.Hide();

        }

        private void ButtonNormalDifficulty_Click(object sender, RoutedEventArgs e)
        {
            var playGameSingleplayer = new PlayGameSingleplayer(DifficultyPresets.Normal);
            playGameSingleplayer.WindowState = this.WindowState;
            playGameSingleplayer.Owner = this;
            playGameSingleplayer.Show();
            this.Hide();
        }

        private void ButtonHardDifficulty_Click(object sender, RoutedEventArgs e)
        {
            var playGameSingleplayer = new PlayGameSingleplayer(DifficultyPresets.Hard);
            playGameSingleplayer.WindowState = this.WindowState;
            playGameSingleplayer.Owner = this;
            playGameSingleplayer.Show();
            this.Hide();
        }

        private void ButtonCustomizeDifficulty_Click(object sender, RoutedEventArgs e)
        {
            var customizeGame = new CustomizeGame();
            customizeGame.WindowState = this.WindowState;
            customizeGame.Owner = this;
            customizeGame.Show();
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
