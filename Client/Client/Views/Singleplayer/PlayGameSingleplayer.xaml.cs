using Client.Helpers;
using Client.Models;
using Client.Properties.Langs;
using Client.Utilities;
using Client.ViewModels;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Client.Views.Singleplayer
{
    /// <summary>
    /// Lógica de interacción para PlayGameSingleplayer.xaml
    /// </summary>
    public partial class PlayGameSingleplayer : Window
    {
        public ObservableCollection<Card> Cards { get; set; }
        public int GameRows { get; set; }
        public int GameColumns { get; set; }
        private readonly GameManager _gameManager;

        public PlayGameSingleplayer(GameConfiguration config)
        {
            InitializeComponent();

            Cards = new ObservableCollection<Card>();
            GameBoard.ItemsSource = Cards;
            GameRows = config.NumberRows;
            GameColumns = config.NumberColumns;

            this.DataContext = this;

            _gameManager = new GameManager(Cards);

            _gameManager.TimerUpdated += OnTimerUpdated;
            _gameManager.ScoreUpdated += OnScoreUpdated;
            _gameManager.GameWon += OnGameWon;
            _gameManager.GameLost += OnGameLost;

            _gameManager.StartSingleplayerGame(config);
        }

        private void OnTimerUpdated(string timeString)
        {
            LabelTimer.Content = timeString;
        }

        private void OnScoreUpdated(int newScore)
        {
            LabelScore.Content = newScore.ToString();
        }

        private void OnGameWon()
        {
            string winnerName = UserSession.Username ?? "Player";
            string statsInfo = $"{Lang.Global_Label_Score} {LabelScore.Content} | {Lang.MatchSummary_Label_TimeRemaining} {LabelTimer.Content}";

            var summaryWindow = new MatchSummary(winnerName, statsInfo);
            summaryWindow.Owner = this;
            summaryWindow.ShowDialog();

            ButtonBackToSelectDifficulty_Click(null, null);
        }

        private void OnGameLost()
        {
            string title = Lang.Singleplayer_Title_TimeOver;
            string statsInfo = $"{Lang.Global_Label_Score}: {LabelScore.Content}";

            var summaryWindow = new MatchSummary(title, statsInfo);
            summaryWindow.Owner = this;
            summaryWindow.ShowDialog();

            ButtonBackToSelectDifficulty_Click(null, null);
        }

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var clickedCard = button.DataContext as Card;

            if (clickedCard != null)
            {
                await _gameManager.HandleCardClick(clickedCard);
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.StopGame();
            Window settingsWindow = new Settings();
            settingsWindow.Owner = this;
            settingsWindow.WindowState = this.WindowState;
            settingsWindow.ShowDialog();
        }

        public void ButtonBackToSelectDifficulty_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.StopGame();
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
