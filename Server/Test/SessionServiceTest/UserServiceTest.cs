using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Test.Helpers;

namespace Test.SessionServiceTest
{
    [TestClass]
    public class UserServiceTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<INotificationService> _mockNotification;
        private Mock<ISecurityService> _mockSecurity;
        private Mock<ISessionManager> _mockSession;

        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockNotification = new Mock<INotificationService>();
            _mockSecurity = new Mock<ISecurityService>();
            _mockSession = new Mock<ISessionManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _userService = new UserService(
                _mockDbFactory.Object,
                _mockNotification.Object,
                _mockSecurity.Object,
                _mockSession.Object
            );
        }

        [TestMethod]
        public void StartRegistration_EmailAlreadyExists_ReturnsEmailInUseError()
        {
            string email = "existe@test.com";

            var usersData = new List<user> { new user { email = email } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(usersData).Object);

            var result = _userService.StartRegistration(email, "Password123!");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Global_Error_EmailInUse", result.MessageKey);
        }

        [TestMethod]
        public void StartRegistration_ValidData_SavesPendingAndSendsEmail()
        {
            string email = "nuevo@test.com";
            string pin = "123456";

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration>().AsQueryable()).Object);

            _mockSecurity.Setup(s => s.GeneratePin()).Returns(pin);
            _mockSecurity.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hashed_pass");
            _mockNotification.Setup(n => n.SendVerificationEmail(email, pin)).Returns(true);

            var result = _userService.StartRegistration(email, "Password123!");

            Assert.IsTrue(result.Success);

            _mockContext.Verify(c => c.pendingRegistration.Add(It.Is<pendingRegistration>(p => p.email == email && p.pin == pin)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void VerifyRegistration_ValidCode_CreatesUserAndReturnsSuccess()
        {
            string email = "nuevo@test.com";
            string pin = "123456";
            var pending = new pendingRegistration
            {
                email = email,
                pin = pin,
                expirationTime = DateTime.Now.AddMinutes(10),
                hashedPassword = "hash"
            };

            var pendingData = new List<pendingRegistration> { pending }.AsQueryable();
            var userData = new List<user>().AsQueryable();

            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(pendingData).Object);
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(userData).Object);

            var result = _userService.VerifyRegistration(email, pin);

            Assert.IsTrue(result.Success);
            _mockContext.Verify(c => c.user.Add(It.Is<user>(u => u.email == email && u.verifiedEmail == true)), Times.Once);
            _mockSecurity.Verify(s => s.RemovePendingRegistration(email), Times.Once);
        }

        [TestMethod]
        public void VerifyRegistration_ExpiredCode_ReturnsError()
        {
            string email = "tarde@test.com";
            var pending = new pendingRegistration
            {
                email = email,
                pin = "123456",
                expirationTime = DateTime.Now.AddMinutes(-5)
            };

            var pendingData = new List<pendingRegistration> { pending }.AsQueryable();
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(pendingData).Object);

            var result = _userService.VerifyRegistration(email, "123456");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Global_Error_CodeInvalid", result.MessageKey);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsToken()
        {
            string email = "gamer@test.com";
            string rawPassword = "Password123!";

            string realBcryptHash = BCrypt.Net.BCrypt.HashPassword(rawPassword);

            var existingUser = new user
            {
                userId = 1,
                email = email,
                password = realBcryptHash,
                username = "GamerPro"
            };

            var userData = new List<user> { existingUser }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(userData).Object);

            _mockSession.Setup(s => s.CreateSessionToken(existingUser.userId)).Returns("token_falso_123");

            var result = _userService.Login(email, rawPassword);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("token_falso_123", result.SessionToken);
            Assert.AreEqual("GamerPro", result.User.Username);
        }

        [TestMethod]
        public void Login_InvalidPassword_ReturnsError()
        {

            string email = "gamer@test.com";
            string realHash = BCrypt.Net.BCrypt.HashPassword("LaContraseñaCorrecta");

            var existingUser = new user { email = email, password = realHash };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { existingUser }.AsQueryable()).Object);

            var result = _userService.Login(email, "ContraseñaIncorrecta");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Global_Error_InvalidCredentials", result.MessageKey);
        }

        [TestMethod]
        public void LoginAsGuest_ValidUsername_CreatesGuestUser()
        {
            string guestName = "Invitado1";

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);

            _mockSecurity.Setup(s => s.GenerateGuestPassword()).Returns("guestPass");
            _mockSecurity.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hash");
            _mockSession.Setup(s => s.CreateSessionToken(It.IsAny<int>())).Returns("token_guest");

            var result = _userService.LoginAsGuest(guestName);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.User.Email.Contains("@guest.local"));

            _mockContext.Verify(c => c.user.Add(It.Is<user>(u => u.isGuest == true && u.username == guestName)), Times.Once);
        }
    }
}