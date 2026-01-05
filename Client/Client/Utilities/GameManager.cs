using Client.GameLobbyServiceReference;
using Client.Models;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Client.Utilities
{
    public class GameManager
    {
        public event Action<string> TimerUpdated;
        public event Action<int> ScoreUpdated;
        public event Action GameWon;
        public event Action GameLost;

        private DispatcherTimer gameTimer;
        private TimeSpan timeLeft;
        private int score;
        private bool isProcessingTurn;
        private Card firstCardFlipped;
        private readonly ObservableCollection<Card> cardsOnBoard;

        public GameManager(ObservableCollection<Card> cardsCollection)
        {
            cardsOnBoard = cardsCollection;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                TimerUpdated?.Invoke(timeLeft.ToString(@"mm\:ss"));
            }
            else
            {
                gameTimer.Stop();
                TimerUpdated?.Invoke("00:00");
                GameLost?.Invoke();
            }
        }
        public void StartSingleplayerGame(GameConfiguration configuration)
        {
            ResetGameState(configuration.TimeLimitSeconds);

            var deck = GenerateRandomDeck(configuration.NumberOfCards);
            foreach (var card in deck)
            {
                cardsOnBoard.Add(card);
            }

            gameTimer.Start();
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
                string fullPath = $"/Client;component/Resources/Images/Cards/Fronts/Color/{imgName}.png";

                deck.Add(new Card(i * 2, i, fullPath));
                deck.Add(new Card(i * 2 + 1, i, fullPath));
            }

            Random rng = new Random();
            return deck.OrderBy(x => rng.Next()).ToList();
        }

        public void StartMultiplayerGame(GameConfiguration configuration, List<CardInfo> serverCards)
        {
            ResetGameState(configuration.TimeLimitSeconds);

            int index = 0;
            foreach (var info in serverCards)
            {
                string imagePath = $"/Client;component/Resources/Images/Cards/Fronts/Color/{info.ImageIdentifier}.png";
                var newCard = new Card(index, info.CardId, imagePath);

                cardsOnBoard.Add(newCard);
                index++;
            }
            gameTimer.Start();
        }
        private void ResetGameState(int seconds)
        {
            timeLeft = TimeSpan.FromSeconds(seconds);
            score = 0;
            firstCardFlipped = null;
            isProcessingTurn = false;
            cardsOnBoard.Clear();

            TimerUpdated?.Invoke(timeLeft.ToString(@"mm\:ss"));
            ScoreUpdated?.Invoke(0);
        }
        public async Task HandleCardClick(Card clickedCard)
        {
            if (isProcessingTurn || clickedCard.IsFlipped || clickedCard.IsMatched) return;

            clickedCard.IsFlipped = true;

            if (firstCardFlipped == null)
            {
                firstCardFlipped = clickedCard;
            }
            else
            {
                isProcessingTurn = true;
                if (firstCardFlipped.PairId == clickedCard.PairId)
                {
                    await Task.Delay(500);
                    firstCardFlipped.IsMatched = true;
                    clickedCard.IsMatched = true;
                    score += 10;
                    ScoreUpdated?.Invoke(score);
                    CheckWinCondition();
                }
                else
                {
                    await Task.Delay(1000);
                    firstCardFlipped.IsFlipped = false;
                    clickedCard.IsFlipped = false;
                }
                firstCardFlipped = null;
                isProcessingTurn = false;
            }
        }

        private void CheckWinCondition()
        {
            if (cardsOnBoard.All(c => c.IsMatched))
            {
                gameTimer.Stop();
                GameWon?.Invoke();
            }
        }

        public void StopGame()
        {
            gameTimer?.Stop();
        }
    }
}