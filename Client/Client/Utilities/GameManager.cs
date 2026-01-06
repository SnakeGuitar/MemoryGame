using Client.GameLobbyServiceReference;
using Client.Models;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Client.Utilities
{
    /// <summary>
    /// Manages the core game logic, including turn processing, score tracking, 
    /// timer management, and win/loss conditions.
    /// </summary>
    public class GameManager
    {
        #region Events
        public event Action<string> TimerUpdated;
        public event Action<int> ScoreUpdated;
        public event Action GameWon;
        public event Action GameLost;
        #endregion

        #region Private Fields
        private DispatcherTimer _gameTimer;
        private TimeSpan _timeLeft;
        private int _score;
        private bool _isProcessingTurn;
        private Card _firstCardFlipped;
        private readonly ObservableCollection<Card> _cardsOnBoard;
        #endregion

        public GameManager(ObservableCollection<Card> cardsCollection)
        {
            _cardsOnBoard = cardsCollection ?? throw new ArgumentNullException(nameof(cardsCollection));
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += GameTimer_Tick;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_timeLeft.TotalSeconds > 0)
                {
                    _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
                    TimerUpdated?.Invoke(_timeLeft.ToString(@"mm\:ss"));
                }
                else
                {
                    StopGame();
                    TimerUpdated?.Invoke("00:00");
                    GameLost?.Invoke();
                }
            }
            catch (Exception ex)
            {
                StopGame();
                GameLost?.Invoke();
                System.Diagnostics.Debug.WriteLine($"[GAME MANAGER] Timer error: {ex.Message}");
            }
        }

        /// <summary>
        /// Starts a singleplayer game with a locally generated random deck.
        /// </summary>
        public void StartSingleplayerGame(GameConfiguration configuration)
        {
            ResetGameState(configuration.TimeLimitSeconds);

            var deck = GenerateRandomDeck(configuration.NumberOfCards);
            foreach (var card in deck)
            {
                _cardsOnBoard.Add(card);
            }

            _gameTimer.Start();
        }

        /// <summary>
        /// Starts a multiplayer game using the synchronized deck provided by the server.
        /// </summary>
        public void StartMultiplayerGame(GameConfiguration configuration, List<CardInfo> serverCards)
        {
            ResetGameState(configuration.TimeLimitSeconds);

            int index = 0;
            foreach (var info in serverCards)
            {
                string imagePath = $"{GameConstants.ColorCardFrontBasePath}{info.ImageIdentifier}.png";
                var newCard = new Card(index, info.CardId, imagePath);

                _cardsOnBoard.Add(newCard);
                index++;
            }
            _gameTimer.Start();
        }

        private void ResetGameState(int seconds)
        {
            _timeLeft = TimeSpan.FromSeconds(seconds);
            _score = 0;
            _firstCardFlipped = null;
            _isProcessingTurn = false;
            _cardsOnBoard.Clear();

            TimerUpdated?.Invoke(_timeLeft.ToString(@"mm\:ss"));
            ScoreUpdated?.Invoke(0);
        }

        /// <summary>
        /// Generates a shuffled deck of cards locally.
        /// </summary>
        private List<Card> GenerateRandomDeck(int numberOfCards)
        {
            List<string> imagePaths = new List<string>
            {
                "africa", "ana", "ari", "blanca", "emily", "fer",
                "katya", "lala", "linda", "paul", "saddy", "sara"
            };

            List<Card> deck = new List<Card>();
            int pairsNeeded = numberOfCards / 2;

            for (int i = 0; i < pairsNeeded; i++)
            {
                string imgName = imagePaths[i % imagePaths.Count];
                string fullPath = $"{GameConstants.ColorCardFrontBasePath}{imgName}.png";

                deck.Add(new Card(i * 2, i, fullPath));
                deck.Add(new Card(i * 2 + 1, i, fullPath));
            }

            return deck.OrderBy(x => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Handles the logic when a card is clicked by the user.
        /// </summary>
        public async Task HandleCardClick(Card clickedCard)
        {
            if (_isProcessingTurn || clickedCard.IsFlipped || clickedCard.IsMatched) return;

            clickedCard.IsFlipped = true;

            if (_firstCardFlipped == null)
            {
                _firstCardFlipped = clickedCard;
            }
            else
            {
                _isProcessingTurn = true;

                if (_firstCardFlipped.PairId == clickedCard.PairId)
                {
                    await Task.Delay(GameConstants.MatchFeedbackDelay);

                    _firstCardFlipped.IsMatched = true;
                    clickedCard.IsMatched = true;

                    _score += GameConstants.PointsPerMatch;
                    ScoreUpdated?.Invoke(_score);

                    CheckWinCondition();
                }
                else
                {
                    await Task.Delay(GameConstants.MismatchFeedbackDelay);

                    _firstCardFlipped.IsFlipped = false;
                    clickedCard.IsFlipped = false;
                }

                _firstCardFlipped = null;
                _isProcessingTurn = false;
            }
        }

        private void CheckWinCondition()
        {
            if (_cardsOnBoard.All(c => c.IsMatched))
            {
                StopGame();
                GameWon?.Invoke();
            }
        }

        public void StopGame()
        {
            _gameTimer?.Stop();
        }
    }
}