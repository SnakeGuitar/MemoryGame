using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.GameService;
using Server.GameService.Core;
using Server.LobbyService;
using System.Collections.Generic;
using System.Threading;

namespace Test.GameServiceTest.CoreTest
{
    [TestClass]
    public class GameNotifierTest
    {
        private Mock<IGameLobbyCallback> _mockCallback;
        private LobbyClient _player;
        private GameNotifier _notifier;

        [TestInitialize]
        public void Setup()
        {
            _mockCallback = new Mock<IGameLobbyCallback>();
            _player = new LobbyClient { Id = "1", Name = "Player1", Callback = _mockCallback.Object };
            _notifier = new GameNotifier(new List<LobbyClient> { _player });
        }

        [TestMethod]
        public void NotifyGameStarted_CallsCallback()
        {
            _notifier.NotifyGameStarted(new List<CardInfo>());
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.GameStarted(It.IsAny<List<CardInfo>>()), Times.Once);
        }

        [TestMethod]
        public void NotifyTurnChange_CallsCallback()
        {
            _notifier.NotifyTurnChange("Player1", 10);
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.UpdateTurn("Player1", 10), Times.Once);
        }

        [TestMethod]
        public void NotifyShowCard_CallsCallback()
        {
            _notifier.NotifyShowCard(1, "img");
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.ShowCard(1, "img"), Times.Once);
        }

        [TestMethod]
        public void NotifyMatch_CallsSetCardsAsMatched()
        {
            _notifier.NotifyMatch(1, 2, "P1", 100);
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.SetCardsAsMatched(1, 2), Times.Once);
        }

        [TestMethod]
        public void NotifyMatch_CallsUpdateScore()
        {
            _notifier.NotifyMatch(1, 2, "P1", 100);
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.UpdateScore("P1", 100), Times.Once);
        }

        [TestMethod]
        public void NotifyHideCards_CallsCallback()
        {
            _notifier.NotifyHideCards(1, 2);
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.HideCards(1, 2), Times.Once);
        }

        [TestMethod]
        public void NotifyWinner_CallsCallback()
        {
            _notifier.NotifyWinner("Winner");
            Thread.Sleep(100);

            _mockCallback.Verify(c => c.GameFinished("Winner"), Times.Once);
        }
    }
}