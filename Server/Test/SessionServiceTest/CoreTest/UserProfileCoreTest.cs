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
    public class UserProfileCoreTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<ISessionManager> _mockSession;
        private Mock<ISecurityService> _mockSecurity;
        private Mock<ILoggerManager> _mockLogger;

        private UserProfileCore _profileCore;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockSession = new Mock<ISessionManager>();
            _mockSecurity = new Mock<ISecurityService>();
            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _profileCore = new UserProfileCore(
                (DbContextFactory)_mockDbFactory.Object,
                _mockLogger.Object,
                _mockSession.Object,
                _mockSecurity.Object
            );
        }

        [TestMethod]
        public void ChangePassword_UserNotFound_ReturnsFalse()
        {
            string token = "token_bad";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(0);

            var result = _profileCore.ChangePassword(token, "old", "new");

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void ChangePassword_IncorrectCurrentPassword_ReturnsInvalidCredentialsError()
        {
            string token = "token_ok";
            string realHash = BCrypt.Net.BCrypt.HashPassword("RealPass");
            var user = new user { userId = 1, password = realHash };
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);

            var result = _profileCore.ChangePassword(token, "WrongPass", "NewPass");

            Assert.AreEqual("Global_Error_InvalidCredentials", result.MessageKey);
        }

        [TestMethod]
        public void ChangePassword_Valid_ReturnsSuccess()
        {
            string token = "token_ok";
            string realHash = BCrypt.Net.BCrypt.HashPassword("OldPass");
            var user = new user { userId = 1, password = realHash };
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);
            _mockSecurity.Setup(s => s.HashPassword("NewPass")).Returns("NewHash");

            var result = _profileCore.ChangePassword(token, "OldPass", "NewPass");

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void ChangeUsername_UsernameTaken_ReturnsFalse()
        {
            string token = "token_ok";
            string newName = "TakenName";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(1);
            var currentUser = new user { userId = 1, username = "Me" };
            var otherUser = new user { userId = 2, username = "TakenName" };
            var users = new List<user> { currentUser, otherUser }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _profileCore.ChangeUsername(token, newName);

            Assert.IsFalse(result.Success);
        }
    }
}