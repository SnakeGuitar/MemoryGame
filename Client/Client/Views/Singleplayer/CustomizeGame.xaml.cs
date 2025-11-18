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
    /// Lógica de interacción para CustomizeGame.xaml
    /// </summary>
    public partial class CustomizeGame : Window
    {
        public CustomizeGame()
        {
            InitializeComponent();
        } 

        private void ComboBoxSelectNumberCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TimerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (LabelTimerValue != null)
            {
                LabelTimerValue.Content = e.NewValue.ToString("F0");
            }
        }

        private void ButtonPlayGame_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Window selectDifficulty = this.Owner;
            

            if (selectDifficulty != null)
            {
                selectDifficulty.WindowState = this.WindowState;
                selectDifficulty.Show();
            }
            this.Close();

        }
    }
}
