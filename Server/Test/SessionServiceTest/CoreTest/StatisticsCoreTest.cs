using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.SessionService.Core;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Helpers;

namespace Test.SessionServiceTest.CoreTest
{
    [TestClass]
    public class StatisticsCoreTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<ISessionManager> _mockSession;
        private Mock<ILoggerManager> _mockLogger;

        private StatisticsCore _statsCore;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockSession = new Mock<ISessionManager>();
            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _statsCore = new StatisticsCore(
                _mockDbFactory.Object,
                _mockSession.Object,
                _mockLogger.Object
            );
        }

        [TestMethod]
        public void GetMatchHistory_InvalidToken_ReturnsNull()
        {
            string token = "invalid";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns((int?)null);

            var result = _statsCore.GetMatchHistory(token);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetMatchHistory_ValidToken_ReturnsList()
        {
            string token = "valid";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var matchData = new match { endDateTime = DateTime.UtcNow };
            var history = new matchHistory
            {
                userId = myId,
                matchId = 100,
                match = matchData,
                winnerId = myId
            };

            var winnerUser = new user { userId = myId, username = "Winner" };

            _mockContext.Setup(c => c.matchHistory).Returns(DbContextMockFactory.CreateMockDbSet(new List<matchHistory> { history }.AsQueryable()).Object);
            _mockContext.Setup(c => c.user.Find(It.IsAny<object[]>())).Returns(winnerUser);

            var result = _statsCore.GetMatchHistory(token);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetMatchHistory_ValidToken_MapsScoreCorrectly()
        {
            string token = "valid";
            int myId = 1;
            int expectedScore = 500;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var history = new matchHistory
            {
                userId = myId,
                score = expectedScore,
                match = new match { endDateTime = DateTime.UtcNow }
            };

            _mockContext.Setup(c => c.matchHistory).Returns(DbContextMockFactory.CreateMockDbSet(new List<matchHistory> { history }.AsQueryable()).Object);

            var result = _statsCore.GetMatchHistory(token);

            Assert.AreEqual(expectedScore, result[0].Score);
        }

        [TestMethod]
        public void GetMatchHistory_DbError_ReturnsNull()
        {
            string token = "valid";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);

            _mockContext.Setup(c => c.matchHistory).Throws(new System.Exception("DB Down"));

            var result = _statsCore.GetMatchHistory(token);

            Assert.IsNull(result);
        }
    }
}