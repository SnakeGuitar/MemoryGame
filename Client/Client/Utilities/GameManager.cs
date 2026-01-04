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

        public void StartNewGame(GameConfiguration configuration)
        {
            timeLeft = TimeSpan.FromSeconds(configuration.TimeLimitSeconds);
            score = 0;
            firstCardFlipped = null;
            isProcessingTurn = false;

            TimerUpdated?.Invoke(timeLeft.ToString(@"mm\:ss"));
            ScoreUpdated?.Invoke(0);
            cardsOnBoard.Clear();

            var deck = GenerateDeck(configuration.NumberOfCards);

            foreach (var card in deck)
            {
                cardsOnBoard.Add(card);
            }
            gameTimer.Start();
        }

        private static List<Card> GenerateDeck(int numberOfCards)
        {
            List<string> imagePaths = new List<string>
            {
                "/Client;component/Resources/Images/Cards/Fronts/Color/africa.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/ana.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/ari.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/blanca.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/emily.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/fer.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/katya.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/lala.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/linda.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/paul.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/saddy.png",
                "/Client;component/Resources/Images/Cards/Fronts/Color/sara.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/africa.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/ana.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/ari.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/blanca.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/emily.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/fer.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/katya.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/lala.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/linda.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/paul.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/saddy.png",
                "/Client;component/Resources/Images/Cards/Fronts/Normal/sara.png"
            };

            List<Card> deck = new List<Card>();
            int pairsNeeded = numberOfCards / 2;

            for (int i = 0; i < pairsNeeded; i++)
            {
                string image = imagePaths[i % imagePaths.Count];
                deck.Add(new Card(i * 2, i, image));
                deck.Add(new Card(i * 2 + 1, i, image));

            }

            Random rng = new Random();
            return deck.OrderBy(x => rng.Next()).ToList();
        }

        public async Task HandleCardClick(Card clickedCard)
        {
            if (isProcessingTurn || clickedCard.IsFlipped || clickedCard.IsMatched)
            {
                return;
            }

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
