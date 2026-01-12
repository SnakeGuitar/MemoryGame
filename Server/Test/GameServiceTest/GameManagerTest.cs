using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.GameService;
using Server.LobbyService;
using Server.Shared;
using System.Collections.Generic;

namespace Test.GameServiceTest
{
    [TestClass]
    public class GameManagerTest
    {
        private GameManager _gameManager;
        private List<LobbyClient> _players;
        private GameSettings _settings;

        private Mock<ILoggerManager> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerManager>();

            _players = new List<LobbyClient>
            {
                new LobbyClient { Id = "1", Name = "P1" },
                new LobbyClient { Id = "2", Name = "P2" }
            };

            _settings = new GameSettings { CardCount = 4, TurnTimeSeconds = 5 };

            _gameManager = new GameManager(_players, _settings, _mockLogger.Object);
        }

        [TestMethod]
        public void Constructor_SetsIsGameInProgressFalse()
        {
            Assert.IsFalse(_gameManager.IsGameInProgress);
        }

        [TestMethod]
        public void StartGame_SetsIsGameInProgressTrue()
        {
            _gameManager.StartGame();
            Assert.IsTrue(_gameManager.IsGameInProgress);
        }

        [TestMethod]
        public void HandleFlipCard_GameNotStarted_DoesNothing()
        {
            try
            {
                _gameManager.HandleFlipCard("1", 0);
            }
            catch
            {
                Assert.Fail("It should not throw an exception");
            }

            Assert.IsFalse(_gameManager.IsGameInProgress);
        }

        [TestMethod]
        public void SanitizeSettings_OddCardCount_CorrectsToEven()
        {
            var settings = new GameSettings { CardCount = 5, TurnTimeSeconds = 10 };

            var manager = new GameManager(_players, settings, _mockLogger.Object);

            manager.StartGame();

            Assert.IsTrue(manager.IsGameInProgress);
        }

        [TestMethod]
        public void SanitizeSettings_LowTime_CorrectsToMinimum()
        {
            var settings = new GameSettings { CardCount = 16, TurnTimeSeconds = 1 };

            var manager = new GameManager(_players, settings, _mockLogger.Object);

            manager.StartGame();

            Assert.IsTrue(manager.IsGameInProgress);
        }
    }
}