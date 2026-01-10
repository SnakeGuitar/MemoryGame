using Client.Helpers;
using Client.Models;
using System.Windows;

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
            NavigationHelper.NavigateTo(this, new PlayGameSingleplayer(DifficultyPresets.Easy));
        }

        private void ButtonNormalDifficulty_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new PlayGameSingleplayer(DifficultyPresets.Normal));
        }

        private void ButtonHardDifficulty_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new PlayGameSingleplayer(DifficultyPresets.Hard));
        }

        private void ButtonCustomizeDifficulty_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, new CustomizeGame());
        }

        private void ButtonBackToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new MainMenu());
        }
    }
}