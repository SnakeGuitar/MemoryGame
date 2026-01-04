using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.GameService;
using Server.GameService.Core;
using Server.Shared;
using System.Linq;

namespace Test.GameServiceTest.CoreTest
{
    [TestClass]
    public class GameDeckTest
    {
        private Mock<ILoggerManager> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerManager>();
        }

        [TestMethod]
        public void Constructor_GeneratesCorrectNumberOfCards()
        {
            int cardCount = 10;
            var deck = new GameDeck(cardCount, _mockLogger.Object);

            var board = deck.GetBoardInfo();

            Assert.AreEqual(cardCount, board.Count);
        }

        [TestMethod]
        public void GetBoardInfo_ReturnsCardInfoList()
        {
            var deck = new GameDeck(4, _mockLogger.Object);

            var result = deck.GetBoardInfo();

            Assert.IsInstanceOfType(result, typeof(System.Collections.Generic.List<CardInfo>));
        }

        [TestMethod]
        public void GetCard_ValidIndex_ReturnsCard()
        {
            var deck = new GameDeck(4, _mockLogger.Object);

            var card = deck.GetCard(0);

            Assert.IsNotNull(card);
        }

        [TestMethod]
        public void GetCard_InvalidIndex_ReturnsNull()
        {
            var deck = new GameDeck(4, _mockLogger.Object);

            var card = deck.GetCard(99);

            Assert.IsNull(card);
        }

        [TestMethod]
        public void IsAllMatched_Initially_ReturnsFalse()
        {
            var deck = new GameDeck(4, _mockLogger.Object);

            bool result = deck.IsAllMatched();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GenerateDeck_CreatesPairs()
        {
            var deck = new GameDeck(2, _mockLogger.Object);

            var board = deck.GetBoardInfo();

            bool isPair = board[0].ImageIdentifier == board[1].ImageIdentifier;

            Assert.IsTrue(isPair);
        }
    }
}