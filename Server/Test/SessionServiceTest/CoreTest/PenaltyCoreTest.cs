using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.SessionService.Core;
using Server.Shared;
using System.Collections.Generic;
using System.Linq;
using Test.Helpers;

namespace Test.SessionServiceTest.CoreTest
{
    [TestClass]
    public class PenaltyCoreTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<ISessionManager> _mockSession;
        private Mock<ILoggerManager> _mockLogger;

        private PenaltyCore _penaltyCore;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockSession = new Mock<ISessionManager>();
            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _penaltyCore = new PenaltyCore(
                _mockDbFactory.Object,
                _mockSession.Object,
                _mockLogger.Object
            );
        }

        [TestMethod]
        public void ReportUser_InvalidToken_ReturnsFalse()
        {
            string token = "invalid";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns((int?)null);

            var result = _penaltyCore.ReportUser(token, "BadUser", 100);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void ReportUser_TargetNotFound_ReturnsError()
        {
            string token = "valid";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);

            var result = _penaltyCore.ReportUser(token, "Ghost", 100);

            Assert.AreEqual("Global_Error_UserNotFound", result.MessageKey);
        }

        [TestMethod]
        public void ReportUser_ValidReport_AddsPenaltyToDb()
        {
            string token = "valid";
            string targetName = "ToxicPlayer";
            int matchId = 123;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);

            var targetUser = new user { userId = 2, username = targetName };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { targetUser }.AsQueryable()).Object);
            _mockContext.Setup(c => c.penalty).Returns(DbContextMockFactory.CreateMockDbSet(new List<penalty>().AsQueryable()).Object);

            _penaltyCore.ReportUser(token, targetName, matchId);

            _mockContext.Verify(c => c.penalty.Add(It.Is<penalty>(p => p.matchId == matchId)), Times.Once);
        }

        [TestMethod]
        public void ReportUser_ValidReport_ReturnsSuccess()
        {
            string token = "valid";
            string targetName = "ToxicPlayer";

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);

            var targetUser = new user { userId = 2, username = targetName };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { targetUser }.AsQueryable()).Object);
            _mockContext.Setup(c => c.penalty).Returns(DbContextMockFactory.CreateMockDbSet(new List<penalty>().AsQueryable()).Object);

            var result = _penaltyCore.ReportUser(token, targetName, 123);

            Assert.IsTrue(result.Success);
        }
    }
}