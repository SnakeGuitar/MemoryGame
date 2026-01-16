using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Models;
using Client.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Assert = NUnit.Framework.Assert;

namespace Client.Test.Core
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class GameManagerTest
    {
        private GameManager _gameManager;
        private ObservableCollection<Card> _cardsCollection;

        [SetUp]
        public void Setup()
        {
            _cardsCollection = new ObservableCollection<Card>();
            _gameManager = new GameManager(_cardsCollection);
        }

        [Test]
        public void Constructor_NullCollection_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new GameManager(null));
        }

        #region StartSingleplayerGame Tests

        [Test]
        public void StartSingleplayerGame_SetsMultiplayerModeToFalse()
        {
            var config = new GameConfiguration { NumberOfCards = 4, TimeLimitSeconds = 60 };

            _gameManager.StartSingleplayerGame(config);

            Assert.That(_gameManager.IsMultiplayerMode, Is.False);
        }

        [Test]
        public void StartSingleplayerGame_GeneratesCorrectNumberOfCards()
        {
            var config = new GameConfiguration { NumberOfCards = 4, TimeLimitSeconds = 60 };

            _gameManager.StartSingleplayerGame(config);

            Assert.That(_cardsCollection.Count, Is.EqualTo(4));
        }

        [Test]
        public void StartSingleplayerGame_GeneratesCorrectNumberOfPairs()
        {
            var config = new GameConfiguration { NumberOfCards = 4, TimeLimitSeconds = 60 };

            _gameManager.StartSingleplayerGame(config);

            var distinctIds = _cardsCollection.Select(c => c.PairId).Distinct().Count();
            Assert.That(distinctIds, Is.EqualTo(2));
        }

        #endregion

        #region StartMultiplayerGame Tests

        [Test]
        public void StartMultiplayerGame_SetsMultiplayerModeToTrue()
        {
            var config = new GameConfiguration { TimeLimitSeconds = 30 };
            var serverCards = new List<CardInfo>();

            _gameManager.StartMultiplayerGame(config, serverCards);

            Assert.That(_gameManager.IsMultiplayerMode, Is.True);
        }

        [Test]
        public void StartMultiplayerGame_MapsServerCardsCorrectly()
        {
            var config = new GameConfiguration { TimeLimitSeconds = 30 };
            var serverCards = new List<CardInfo>
            {
                new CardInfo { CardId = 0, ImageIdentifier = "img1" },
                new CardInfo { CardId = 0, ImageIdentifier = "img1" }
            };

            _gameManager.StartMultiplayerGame(config, serverCards);

            Assert.That(_cardsCollection.Count, Is.EqualTo(2));
        }

        #endregion

        #region HandleCardClick Tests

        [Test]
        public async Task HandleCardClick_FirstCard_FlipsCard()
        {
            SetupSimpleDeck();
            var card1 = _cardsCollection[0];

            await _gameManager.HandleCardClick(card1);

            Assert.That(card1.IsFlipped, Is.True);
        }

        [Test]
        public async Task HandleCardClick_FirstCard_DoesNotUpdateScore()
        {
            SetupSimpleDeck();
            var card1 = _cardsCollection[0];
            int scoreEvents = 0;
            _gameManager.ScoreUpdated += (s) => scoreEvents++;

            await _gameManager.HandleCardClick(card1);

            Assert.That(scoreEvents, Is.EqualTo(0));
        }

        [Test]
        public async Task HandleCardClick_Match_MarksCardAsMatched()
        {
            SetupSimpleDeck();
            var card1 = _cardsCollection[0];
            var card2 = _cardsCollection[1];

            await _gameManager.HandleCardClick(card1);
            await _gameManager.HandleCardClick(card2);
            await Task.Delay(1500);

            Assert.That(card1.IsMatched, Is.True);
        }

        [Test]
        public async Task HandleCardClick_Match_UpdatesScore()
        {
            SetupSimpleDeck();
            var card1 = _cardsCollection[0];
            var card2 = _cardsCollection[1];
            int currentScore = 0;
            _gameManager.ScoreUpdated += (s) => currentScore = s;

            await _gameManager.HandleCardClick(card1);
            await _gameManager.HandleCardClick(card2);
            await Task.Delay(1500);

            Assert.That(currentScore, Is.GreaterThan(0));
        }

        [Test]
        public async Task HandleCardClick_Mismatch_UnflipsCards()
        {
            SetupSimpleDeck();
            var card1 = _cardsCollection[0];
            var card3 = _cardsCollection[2];

            await _gameManager.HandleCardClick(card1);
            await _gameManager.HandleCardClick(card3);
            await Task.Delay(1500);

            Assert.That(card1.IsFlipped, Is.False);
        }

        [Test]
        public async Task HandleCardClick_Mismatch_DoesNotMarkAsMatched()
        {
            SetupSimpleDeck();
            var card1 = _cardsCollection[0];
            var card3 = _cardsCollection[2];

            await _gameManager.HandleCardClick(card1);
            await _gameManager.HandleCardClick(card3);
            await Task.Delay(1500);

            Assert.That(card1.IsMatched, Is.False);
        }

        [Test]
        public async Task HandleCardClick_WinningGame_FiresGameWonEvent()
        {
            var config = new GameConfiguration { NumberOfCards = 2, TimeLimitSeconds = 60 };
            _gameManager.StartSingleplayerGame(config);
            bool gameWonFired = false;
            _gameManager.GameWon += () => gameWonFired = true;
            var card1 = _cardsCollection[0];
            var card2 = _cardsCollection[1];

            await _gameManager.HandleCardClick(card1);
            await _gameManager.HandleCardClick(card2);
            await Task.Delay(1500);

            Assert.That(gameWonFired, Is.True);
        }

        #endregion

        #region Timer Tests

        [Test]
        public void TimerTick_DecrementsTimeCorrectly()
        {
            var config = new GameConfiguration { NumberOfCards = 4, TimeLimitSeconds = 60 };
            _gameManager.StartSingleplayerGame(config);
            string lastTimeStr = "";
            _gameManager.TimerUpdated += (t) => lastTimeStr = t;

            InvokePrivateMethod(_gameManager, "GameTimer_Tick", null, EventArgs.Empty);

            Assert.That(lastTimeStr, Is.EqualTo("00:59"));
        }

        [Test]
        public void TimerTick_Singleplayer_TimeZero_FiresGameLost()
        {
            var config = new GameConfiguration { NumberOfCards = 4, TimeLimitSeconds = 1 };
            _gameManager.StartSingleplayerGame(config);
            bool gameLostFired = false;
            _gameManager.GameLost += () => gameLostFired = true;

            SetPrivateField(_gameManager, "_timeLeft", TimeSpan.FromSeconds(0));

            InvokePrivateMethod(_gameManager, "GameTimer_Tick", null, EventArgs.Empty);

            Assert.That(gameLostFired, Is.True);
        }

        [Test]
        public void TimerTick_Multiplayer_TimeZero_FiresTurnTimeEnded()
        {
            var config = new GameConfiguration { TimeLimitSeconds = 1 };
            _gameManager.StartMultiplayerGame(config, new List<CardInfo>());
            bool turnEndedFired = false;
            _gameManager.TurnTimeEnded += () => turnEndedFired = true;

            SetPrivateField(_gameManager, "_timeLeft", TimeSpan.FromSeconds(0));

            InvokePrivateMethod(_gameManager, "GameTimer_Tick", null, EventArgs.Empty);

            Assert.That(turnEndedFired, Is.True);
        }

        [Test]
        public void TimerTick_Multiplayer_TimeZero_DoesNotFireGameLost()
        {
            var config = new GameConfiguration { TimeLimitSeconds = 1 };
            _gameManager.StartMultiplayerGame(config, new List<CardInfo>());
            bool gameLostFired = false;
            _gameManager.GameLost += () => gameLostFired = true;

            SetPrivateField(_gameManager, "_timeLeft", TimeSpan.FromSeconds(0));

            InvokePrivateMethod(_gameManager, "GameTimer_Tick", null, EventArgs.Empty);

            Assert.That(gameLostFired, Is.False);
        }

        #endregion

        #region Helpers

        private void SetupSimpleDeck()
        {
            _cardsCollection.Clear();
            _cardsCollection.Add(new Card(0, 0, "path"));
            _cardsCollection.Add(new Card(1, 0, "path"));
            _cardsCollection.Add(new Card(2, 1, "path"));
            _cardsCollection.Add(new Card(3, 1, "path"));
        }

        private void InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            var type = instance.GetType();
            var methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null) throw new ArgumentException($"Method {methodName} not found");
            methodInfo.Invoke(instance, parameters);
        }

        private void SetPrivateField(object instance, string fieldName, object value)
        {
            var type = instance.GetType();
            var fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null) throw new ArgumentException($"Field {fieldName} not found");
            fieldInfo.SetValue(instance, value);
        }

        #endregion
    }
}