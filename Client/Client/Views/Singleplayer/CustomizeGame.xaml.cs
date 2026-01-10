using Client.Helpers;
using Client.Models;
using Client.Properties.Langs;
using System.Windows;
using System.Windows.Controls;

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
            ComboBoxSelectNumberCards.SelectedIndex = 0;
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
            int selectedCards = 16;

            if (ComboBoxSelectNumberCards.SelectedItem is ComboBoxItem selectedItem &&
                int.TryParse(selectedItem.Content.ToString(), out int result))
            {
                selectedCards = result;
            }

            int selectedTime = (int)TimerSlider.Value;
            var (Rows, Columns) = DifficultyPresets.CalculateLayout(selectedCards);

            var customConfig = new GameConfiguration
            {
                NumberOfCards = selectedCards,
                TimeLimitSeconds = selectedTime,
                DifficultyLevel = Lang.Global_Button_Custom,
                NumberRows = Rows,
                NumberColumns = Columns
            };

            var gameWindow = new PlayGameSingleplayer(customConfig);
        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new SelectDifficulty());
        }
    }
}