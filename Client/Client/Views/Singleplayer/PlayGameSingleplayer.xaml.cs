using Client.Core;
using Client.Helpers;
using Client.Models;
using Client.Properties.Langs;
using Client.ViewModels;
using Client.Views.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views.Singleplayer
{
    /// <summary>
    /// Interaction logic for PlayGameSingleplayer.xaml.
    /// Manages the local game loop, UI updates, and user interaction for single-player mode.
    /// </summary>
    public partial class PlayGameSingleplayer : Window
    {
        #region Properties
        public ObservableCollection<Card> Cards { get; set; }
        public int GameRows { get; set; }
        public int GameColumns { get; set; }

        private readonly GameManager _gameManager;
        #endregion

        public PlayGameSingleplayer(GameConfiguration config)
        {
            InitializeComponent();

            // Safety fallback for layout config
            if (config.NumberRows * config.NumberColumns != config.NumberOfCards)
            {
                config.NumberRows = 4;
                config.NumberColumns = 4;
                config.NumberOfCards = 16;
            }

            Cards = new ObservableCollection<Card>();
            GameBoard.ItemsSource = Cards;
            GameRows = config.NumberRows;
            GameColumns = config.NumberColumns;

            this.DataContext = this;

            _gameManager = new GameManager(Cards);
            ConfigureGameEvents();

            try
            {
                _gameManager.StartSingleplayerGame(config);
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => this.Close());
            }
        }

        #region Event Configuration

        private void ConfigureGameEvents()
        {
            _gameManager.TimerUpdated += OnTimerUpdated;
            _gameManager.ScoreUpdated += OnScoreUpdated;
            _gameManager.GameWon += OnGameWon;
            _gameManager.GameLost += OnGameLost;
        }

        private void UnsubscribeEvents()
        {
            _gameManager.TimerUpdated -= OnTimerUpdated;
            _gameManager.ScoreUpdated -= OnScoreUpdated;
            _gameManager.GameWon -= OnGameWon;
            _gameManager.GameLost -= OnGameLost;
        }

        #endregion

        #region Game Event Handlers

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

            ShowMatchSummary(winnerName, statsInfo);
        }

        private void OnGameLost()
        {
            string title = Lang.Singleplayer_Title_TimeOver;
            string statsInfo = $"{Lang.Global_Label_Score}: {LabelScore.Content}";

            ShowMatchSummary(title, statsInfo);
        }

        private void ShowMatchSummary(string title, string stats)
        {
            var summaryWindow = new MatchSummary(title, stats);
            NavigationHelper.ShowDialog(this, summaryWindow);
            ButtonBackToSelectDifficulty_Click(null, null);
        }

        #endregion

        #region UI Interactions

        private async void Card_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card clickedCard)
            {
                try
                {
                    await _gameManager.HandleCardClick(clickedCard);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Handle(ex, this);
                }
            }
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.StopGame();
            var settingsWindow = new Settings();
            NavigationHelper.ShowDialog(this, settingsWindow);
        }

        public void ButtonBackToSelectDifficulty_Click(object sender, RoutedEventArgs e)
        {
            UnsubscribeEvents();
            _gameManager.StopGame();
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new SelectDifficulty());
        }

        protected override void OnClosed(EventArgs e)
        {
            UnsubscribeEvents();
            _gameManager.StopGame();
            if (this.Owner != null && Application.Current.MainWindow != this.Owner)
            {
                this.Owner.Show();
            }

            base.OnClosed(e);
        }

        #endregion
    }
}