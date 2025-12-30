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
    public class AuthenticationCoreTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<ISecurityService> _mockSecurity;
        private Mock<ISessionManager> _mockSession;
        private Mock<INotificationService> _mockNotification;
        private Mock<ILoggerManager> _mockLogger;

        private AuthenticationCore _authCore;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockSecurity = new Mock<ISecurityService>();
            _mockSession = new Mock<ISessionManager>();
            _mockNotification = new Mock<INotificationService>();
            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _authCore = new AuthenticationCore(
                _mockDbFactory.Object,
                _mockSecurity.Object,
                _mockSession.Object,
                _mockNotification.Object,
                _mockLogger.Object
            );
        }

        // --- StartRegistration ---

        [TestMethod]
        public void StartRegistration_EmailInUse_ReturnsFalse()
        {
            string email = "existe@test.com";
            var users = new List<user> { new user { email = email } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.StartRegistration(email, "Pass123!");

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void StartRegistration_EmailInUse_ReturnsSpecificMessage()
        {
            string email = "existe@test.com";
            var users = new List<user> { new user { email = email } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.StartRegistration(email, "Pass123!");

            Assert.AreEqual("Global_Error_EmailInUse", result.MessageKey);
        }

        [TestMethod]
        public void StartRegistration_ValidData_ReturnsSuccess()
        {
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration>().AsQueryable()).Object);
            _mockNotification.Setup(n => n.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _authCore.StartRegistration("new@test.com", "Pass123!");

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void StartRegistration_ValidData_SavesPendingRegistrationToDb()
        {
            string email = "new@test.com";
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);
            var mockSet = DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration>().AsQueryable());
            _mockContext.Setup(c => c.pendingRegistration).Returns(mockSet.Object);
            _mockNotification.Setup(n => n.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            _authCore.StartRegistration(email, "Pass123!");

            _mockContext.Verify(c => c.pendingRegistration.Add(It.Is<pendingRegistration>(p => p.email == email)), Times.Once);
        }

        // --- VerifyRegistration ---

        [TestMethod]
        public void VerifyRegistration_ValidCode_ReturnsSuccess()
        {
            string email = "valid@test.com";
            var pending = new pendingRegistration { email = email, pin = "123456", expirationTime = DateTime.Now.AddMinutes(10) };
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration> { pending }.AsQueryable()).Object);
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);

            var result = _authCore.VerifyRegistration(email, "123456");

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void VerifyRegistration_ExpiredCode_ReturnsFalse()
        {
           var pending = new pendingRegistration
            {
                email = "old@test.com",
                pin = "123456",
                expirationTime = DateTime.Now.AddMinutes(-10) // Expirado
            };
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration> { pending }.AsQueryable()).Object);

            var result = _authCore.VerifyRegistration("old@test.com", "123456");

            Assert.IsFalse(result.Success);
        }

        // --- Login ---

        [TestMethod]
        public void Login_ValidCredentials_ReturnsSuccess()
        {
            string email = "gamer@test.com";
            string hash = BCrypt.Net.BCrypt.HashPassword("Pass123!");
            var user = new user { userId = 1, email = email, password = hash };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);
            _mockSession.Setup(s => s.CreateSessionToken(1)).Returns("token_valid");
            
            var result = _authCore.Login(email, "Pass123!");

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsToken()
        {
            string email = "gamer@test.com";
            string hash = BCrypt.Net.BCrypt.HashPassword("Pass123!");
            var user = new user { userId = 1, email = email, password = hash };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);
            _mockSession.Setup(s => s.CreateSessionToken(1)).Returns("el_token_generado");

            var result = _authCore.Login(email, "Pass123!");

            Assert.AreEqual("el_token_generado", result.SessionToken);
        }
    }
}