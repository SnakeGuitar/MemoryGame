using Client.GameLobbyServiceReference;
using Client.Models;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Client.Core
{
    /// <summary>
    /// Manages game state, timer, and card interactions.
    /// In Multiplayer, acts as a visual state holder.
    /// In Singleplayer, controls the game rules.
    /// </summary>
    public class GameManager
    {
        #region Events

        public event Action<string> TimerUpdated;
        public event Action<int> ScoreUpdated;
        public event Action GameWon;
        public event Action GameLost;
        public event Action TurnTimeEnded;

        #endregion

        #region Private Fields

        private DispatcherTimer _gameTimer;
        private TimeSpan _timeLeft;
        private int _score;
        private bool _isProcessingTurn;
        private Card _firstCardFlipped;
        private readonly ObservableCollection<Card> _cardsOnBoard;
        private int _turnDurationSeconds;

        #endregion

        public bool IsMultiplayerMode { get; set; } = false;

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
                    HandleTimerExpiration();
                }
            }
            catch (Exception ex)
            {
                StopGame();
                System.Diagnostics.Debug.WriteLine($"[GAME MANAGER] Timer error: {ex.Message}");
            }
        }

        private void HandleTimerExpiration()
        {
            if (IsMultiplayerMode)
            {
                _gameTimer.Stop();
                TimerUpdated?.Invoke("00:00");
                TurnTimeEnded?.Invoke();
            }
            else
            {
                StopGame();
                TimerUpdated?.Invoke("00:00");
                GameLost?.Invoke();
            }
        }

        public void StartSingleplayerGame(GameConfiguration configuration)
        {
            IsMultiplayerMode = false;
            ResetGameState(configuration.TimeLimitSeconds);

            var deck = GenerateRandomDeck(configuration.NumberOfCards);
            foreach (var card in deck)
            {
                _cardsOnBoard.Add(card);
            }

            _gameTimer.Start();
        }

        public void StartMultiplayerGame(GameConfiguration configuration, List<CardInfo> serverCards)
        {
            IsMultiplayerMode = true;
            _turnDurationSeconds = configuration.TimeLimitSeconds;

            ResetGameState(_turnDurationSeconds);

            int index = 0;
            foreach (var info in serverCards)
            {
                string imagePath = $"{GameConstants.ColorCardFrontBasePath}{info.ImageIdentifier}.png";
                var newCard = new Card(index, info.CardId, imagePath);

                newCard.IsFlipped = false;
                _cardsOnBoard.Add(newCard);
                index++;
            }
        }

        public void UpdateTurnDuration(int seconds)
        {
            _turnDurationSeconds = seconds;
        }

        public void ResetTurnTimer()
        {
            _timeLeft = TimeSpan.FromSeconds(_turnDurationSeconds);
            TimerUpdated?.Invoke(_timeLeft.ToString(@"mm\:ss"));

            _gameTimer.Stop();
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
        /// Handles card clicks ONLY for Singleplayer mode.
        /// Multiplayer clicks are handled by the GameServiceManager directly in the View.
        /// </summary>
        public async Task HandleCardClick(Card clickedCard)
        {
            if (IsMultiplayerMode)
            {
                return;
            }

            if (_isProcessingTurn || clickedCard.IsFlipped || clickedCard.IsMatched)
            {
                return;
            }

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
                    await ProcessMatch(clickedCard);
                }
                else
                {
                    await ProcessMismatch(clickedCard);
                }

                _firstCardFlipped = null;
                _isProcessingTurn = false;
            }
        }

        private async Task ProcessMatch(Card secondCard)
        {
            await Task.Delay(GameConstants.MatchFeedbackDelay);

            _firstCardFlipped.IsMatched = true;
            secondCard.IsMatched = true;

            _score += GameConstants.PointsPerMatch;
            ScoreUpdated?.Invoke(_score);

            CheckWinCondition();
        }

        private async Task ProcessMismatch(Card secondCard)
        {
            await Task.Delay(GameConstants.MismatchFeedbackDelay);

            _firstCardFlipped.IsFlipped = false;
            secondCard.IsFlipped = false;
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