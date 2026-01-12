using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.GameService;
using Server.GameService.Core;
using Server.LobbyService;
using Server.Shared;
using System.Collections.Generic;
using System.Threading; // Necesario para Thread.Sleep

namespace Test.GameServiceTest.CoreTest
{
    [TestClass]
    public class GameNotifierTest
    {
        private GameNotifier _notifier;
        private List<LobbyClient> _players;

        private Mock<ILoggerManager> _mockLogger;
        private Mock<IGameLobbyCallback> _mockCallback1;
        private Mock<IGameLobbyCallback> _mockCallback2;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerManager>();

            _mockCallback1 = new Mock<IGameLobbyCallback>();
            _mockCallback2 = new Mock<IGameLobbyCallback>();

            var client1 = new LobbyClient { Id = "P1", Name = "Player1" };
            client1.Callback = _mockCallback1.Object;

            var client2 = new LobbyClient { Id = "P2", Name = "Player2" };
            client2.Callback = _mockCallback2.Object;

            _players = new List<LobbyClient> { client1, client2 };

            _notifier = new GameNotifier(_players, _mockLogger.Object);
        }

        [TestMethod]
        public void NotifyGameStarted_CallsCallbackOnAllPlayers()
        {
            var board = new List<CardInfo>();

            _notifier.NotifyGameStarted(board);

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.GameStarted(It.IsAny<List<CardInfo>>()), Times.Once);
            _mockCallback2.Verify(c => c.GameStarted(It.IsAny<List<CardInfo>>()), Times.Once);
        }

        [TestMethod]
        public void NotifyTurnChange_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyTurnChange("PlayerOne", 30);

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.UpdateTurn("PlayerOne", 30), Times.Once);
            _mockCallback2.Verify(c => c.UpdateTurn("PlayerOne", 30), Times.Once);
        }

        [TestMethod]
        public void NotifyShowCard_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyShowCard(1, "img1");

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.ShowCard(1, "img1"), Times.Once);
            _mockCallback2.Verify(c => c.ShowCard(1, "img1"), Times.Once);
        }

        [TestMethod]
        public void NotifyHideCards_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyHideCards(1, 2);

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.HideCards(1, 2), Times.Once);
            _mockCallback2.Verify(c => c.HideCards(1, 2), Times.Once);
        }

        [TestMethod]
        public void NotifyMatch_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyMatch(1, 2, "Winner", 100);

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.SetCardsAsMatched(1, 2), Times.Once);
            _mockCallback1.Verify(c => c.UpdateScore("Winner", 100), Times.Once);

            _mockCallback2.Verify(c => c.SetCardsAsMatched(1, 2), Times.Once);
            _mockCallback2.Verify(c => c.UpdateScore("Winner", 100), Times.Once);
        }

        [TestMethod]
        public void NotifyWinner_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyWinner("Champion");

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.GameFinished("Champion"), Times.Once);
            _mockCallback2.Verify(c => c.GameFinished("Champion"), Times.Once);
        }

        [TestMethod]
        public void NotifyChatMessage_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyChatMessage("Sender", "Msg", false);

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.ReceiveChatMessage("Sender", "Msg", false), Times.Once);
            _mockCallback2.Verify(c => c.ReceiveChatMessage("Sender", "Msg", false), Times.Once);
        }

        [TestMethod]
        public void NotifyPlayerLeft_CallsCallbackOnAllPlayers()
        {
            _notifier.NotifyPlayerLeft("Leaver");

            Thread.Sleep(200);

            _mockCallback1.Verify(c => c.PlayerLeft("Leaver"), Times.Once);
            _mockCallback2.Verify(c => c.PlayerLeft("Leaver"), Times.Once);
        }
    }
}