using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using Test.Helpers;

namespace Test.SessionServiceTest
{
    [TestClass]
    public class SessionManagerTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private SessionManager _sessionManager;
        private Mock<ILoggerManager> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();

            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _sessionManager = new SessionManager(_mockDbFactory.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void CreateSessionToken_ValidUser_AddsSessionAndReturnsToken()
        {
            int userId = 10;
            var mockSet = DbContextMockFactory.CreateMockDbSet(new List<userSession>().AsQueryable());
            _mockContext.Setup(c => c.userSession).Returns(mockSet.Object);

            string token = _sessionManager.CreateSessionToken(userId);

            Assert.IsNotNull(token);
            Assert.AreEqual(32, token.Length);

            mockSet.Verify(m => m.Add(It.Is<userSession>(s => s.userId == userId && s.token == token)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetUserIdFromToken_ValidActiveToken_ReturnsUserId()
        {
            string token = "validtoken123";
            int expectedId = 5;
            var sessions = new List<userSession>
            {
                new userSession
                {
                    token = token,
                    userId = expectedId,
                    expiresAt = DateTime.Now.AddHours(1)
                }
            }.AsQueryable();

            _mockContext.Setup(c => c.userSession).Returns(DbContextMockFactory.CreateMockDbSet(sessions).Object);

            int? result = _sessionManager.GetUserIdFromToken(token);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedId, result.Value);
        }

        [TestMethod]
        public void GetUserIdFromToken_ExpiredToken_ReturnsNull()
        {
            string token = "expiredtoken";
            var sessions = new List<userSession>
            {
                new userSession
                {
                    token = token,
                    userId = 5,
                    expiresAt = DateTime.Now.AddMinutes(-10)
                }
            }.AsQueryable();

            _mockContext.Setup(c => c.userSession).Returns(DbContextMockFactory.CreateMockDbSet(sessions).Object);

            int? result = _sessionManager.GetUserIdFromToken(token);

            Assert.IsNull(result, "Debe devolver null si la sesión expiró");
        }
    }
}