using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.SessionService.Core;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
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

            _mockSecurity.Setup(s => s.HashPassword(It.IsAny<string>())).Returns("hashed_password");
            _mockSecurity.Setup(s => s.GeneratePin()).Returns("123456");
            _mockSecurity.Setup(s => s.GenerateGuestPassword()).Returns("GuestPass123!");
            _mockSession.Setup(s => s.CreateSessionToken(It.IsAny<int>())).Returns("valid_token");

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user>().AsQueryable()).Object);
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration>().AsQueryable()).Object);
            _mockContext.Setup(c => c.userSession).Returns(DbContextMockFactory.CreateMockDbSet(new List<userSession>().AsQueryable()).Object);
            _mockContext.Setup(c => c.matchHistory).Returns(DbContextMockFactory.CreateMockDbSet(new List<matchHistory>().AsQueryable()).Object);

            _authCore = new AuthenticationCore(
                _mockDbFactory.Object,
                _mockSecurity.Object,
                _mockSession.Object,
                _mockNotification.Object,
                _mockLogger.Object
            );
        }

        #region StartRegistration

        [TestMethod]
        public void StartRegistration_InvalidPassword_ReturnsPasswordInvalid()
        {
            var result = _authCore.StartRegistration("test@email.com", "123");
            Assert.AreEqual("Global_Error_PasswordInvalid", result.MessageKey);
        }

        [TestMethod]
        public void StartRegistration_EmailInUse_ReturnsEmailInUse()
        {
            var users = new List<user> { new user { email = "exists@test.com" } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.StartRegistration("exists@test.com", "ValidPass123!");
            Assert.AreEqual("Global_Error_EmailInUse", result.MessageKey);
        }

        [TestMethod]
        public void StartRegistration_EmailSendFails_ReturnsEmailSendFailed()
        {
            _mockNotification.Setup(n => n.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            var result = _authCore.StartRegistration("new@test.com", "ValidPass123!");
            Assert.AreEqual("Global_Error_EmailSendFailed", result.MessageKey);
        }

        [TestMethod]
        public void StartRegistration_Success_ReturnsTrue()
        {
            _mockNotification.Setup(n => n.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _authCore.StartRegistration("new@test.com", "ValidPass123!");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void StartRegistration_DBError_ReturnsServiceError()
        {
            _mockContext.Setup(c => c.SaveChanges()).Throws(new EntityException());

            var result = _authCore.StartRegistration("new@test.com", "ValidPass123!");
            Assert.AreEqual("Global_ServiceError_Database", result.MessageKey);
        }

        #endregion

        #region ResendVerificationCode

        [TestMethod]
        public void ResendVerificationCode_EmailRegistered_ReturnsEmailInUse()
        {
            var users = new List<user> { new user { email = "reg@test.com" } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.ResendVerificationCode("reg@test.com");
            Assert.AreEqual("Global_Error_EmailInUse", result.MessageKey);
        }

        [TestMethod]
        public void ResendVerificationCode_PendingNotFound_ReturnsRegistrationNotFound()
        {
            var result = _authCore.ResendVerificationCode("unknown@test.com");
            Assert.AreEqual("Global_Error_RegistrationNotFound", result.MessageKey);
        }

        [TestMethod]
        public void ResendVerificationCode_Success_ReturnsTrue()
        {
            var pending = new List<pendingRegistration> { new pendingRegistration { email = "p@test.com" } }.AsQueryable();
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(pending).Object);
            _mockNotification.Setup(n => n.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _authCore.ResendVerificationCode("p@test.com");
            Assert.IsTrue(result.Success);
        }

        #endregion

        #region VerifyRegistration

        [TestMethod]
        public void VerifyRegistration_CodeNotFound_ReturnsCodeInvalid()
        {
            var result = _authCore.VerifyRegistration("test@email.com", "wrong");
            Assert.AreEqual("Global_Error_CodeInvalid", result.MessageKey);
        }

        [TestMethod]
        public void VerifyRegistration_CodeExpired_ReturnsCodeExpired()
        {
            var pending = new List<pendingRegistration>
            {
                new pendingRegistration { email = "t@e.com", pin = "123", expirationTime = DateTime.UtcNow.AddMinutes(-1) }
            }.AsQueryable();
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(pending).Object);

            var result = _authCore.VerifyRegistration("t@e.com", "123");
            Assert.AreEqual("Global_Error_CodeExpired", result.MessageKey);
        }

        [TestMethod]
        public void VerifyRegistration_Success_ReturnsTrue()
        {
            var pending = new List<pendingRegistration>
            {
                new pendingRegistration { email = "t@e.com", pin = "123", expirationTime = DateTime.UtcNow.AddMinutes(1), hashedPassword = "hash" }
            }.AsQueryable();
            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(pending).Object);

            var result = _authCore.VerifyRegistration("t@e.com", "123");
            Assert.IsTrue(result.Success);
        }

        #endregion

        #region FinalizeRegistration

        [TestMethod]
        public void FinalizeRegistration_UsernameUsed_ReturnsInvalidUsername()
        {
            var users = new List<user> { new user { email = "me@t.com" }, new user { username = "taken" } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.FinalizeRegistration("me@t.com", "taken", null);
            Assert.AreEqual("Global_Error_InvalidUsername", result.MessageKey);
        }

        [TestMethod]
        public void FinalizeRegistration_InvalidFormat_ReturnsInvalidChars()
        {
            var users = new List<user> { new user { email = "me@t.com" } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            // "a" is too short or invalid depending on validator logic
            var result = _authCore.FinalizeRegistration("me@t.com", "a", null);
            Assert.AreEqual("Global_ValidationUsername_InvalidChars", result.MessageKey);
        }

        [TestMethod]
        public void FinalizeRegistration_Success_ReturnsTrue()
        {
            var users = new List<user> { new user { userId = 1, email = "me@t.com", socialNetwork = new List<socialNetwork>() } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.FinalizeRegistration("me@t.com", "ValidUser", null);
            Assert.IsTrue(result.Success);
        }

        #endregion

        #region Login

        [TestMethod]
        public void Login_UserNotFound_ReturnsInvalidCredentials()
        {
            var result = _authCore.Login("no@t.com", "pass");
            Assert.AreEqual("Global_Error_InvalidCredentials", result.MessageKey);
        }

        [TestMethod]
        public void Login_Penalized_ReturnsAccountPenalized()
        {
            // Note: Since BCrypt is static, we mock the logic flow by assuming password matches if user exists,
            // but we can't easily mock BCrypt.Verify without wrapper. 
            // In integration tests, use valid hash. Here we test the penalty branch logic.
            // Assumption: The code reaches penalty check.
            string hash = BCrypt.Net.BCrypt.HashPassword("Pass123!");
            var user = new user
            {
                email = "banned@t.com",
                password = hash,
                penaltyId = 1,
                penalty = new penalty { duration = DateTime.UtcNow.AddHours(1) }
            };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);

            var result = _authCore.Login("banned@t.com", "Pass123!");
            Assert.AreEqual("Global_Error_AccountPenalized", result.MessageKey);
        }

        [TestMethod]
        public void Login_Success_ReturnsToken()
        {
            string hash = BCrypt.Net.BCrypt.HashPassword("Pass123!");
            var user = new user { userId = 1, email = "ok@t.com", password = hash, socialNetwork = new List<socialNetwork>() };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);

            var result = _authCore.Login("ok@t.com", "Pass123!");
            Assert.AreEqual("valid_token", result.SessionToken);
        }

        #endregion

        #region LoginAsGuest

        [TestMethod]
        public void LoginAsGuest_InvalidUsername_ReturnsInvalidUsername()
        {
            var result = _authCore.LoginAsGuest("a");
            Assert.AreEqual("Global_Error_InvalidUsername", result.MessageKey);
        }

        [TestMethod]
        public void LoginAsGuest_UsernameInUse_ReturnsUsernameInUse()
        {
            var users = new List<user> { new user { username = "taken" } }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _authCore.LoginAsGuest("taken");
            Assert.AreEqual("Global_Error_UsernameInUse", result.MessageKey);
        }

        [TestMethod]
        public void LoginAsGuest_Success_ReturnsToken()
        {
            var result = _authCore.LoginAsGuest("Guest123");
            Assert.AreEqual("valid_token", result.SessionToken);
        }

        #endregion

        #region LogoutGuest

        [TestMethod]
        public void LogoutGuest_InvalidToken_Returns()
        {
            _mockSession.Setup(s => s.GetUserIdFromToken("bad")).Returns((int?)null);

            // Does not throw
            _authCore.LogoutGuest("bad");
            _mockContext.Verify(c => c.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void LogoutGuest_Valid_RemovesData()
        {
            _mockSession.Setup(s => s.GetUserIdFromToken("tok")).Returns(99);
            var guest = new user { userId = 99, isGuest = true };
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { guest }.AsQueryable()).Object);

            _authCore.LogoutGuest("tok");
            _mockContext.Verify(c => c.user.Remove(guest), Times.Once);
        }

        #endregion

        #region Logout

        [TestMethod]
        public void Logout_ValidToken_RemovesSession()
        {
            var session = new userSession { token = "tok" };
            var mockSet = DbContextMockFactory.CreateMockDbSet(new List<userSession> { session }.AsQueryable());
            _mockContext.Setup(c => c.userSession).Returns(mockSet.Object);

            _authCore.Logout("tok");
            mockSet.Verify(m => m.Remove(session), Times.Once);
        }

        #endregion

        #region InitiateGuestRegistration

        [TestMethod]
        public void InitiateGuestRegistration_UserNotFound_ReturnsError()
        {
            var result = _authCore.InitiateGuestRegistration(999, "e@m.com", "Pass123!");
            Assert.AreEqual("Global_Error_UserNotFound", result.MessageKey);
        }

        [TestMethod]
        public void InitiateGuestRegistration_NotGuest_ReturnsAlreadyRegistered()
        {
            var user = new user { userId = 1, isGuest = false };
            _mockContext.Setup(c => c.user.Find(1)).Returns(user);

            var result = _authCore.InitiateGuestRegistration(1, "e@m.com", "Pass123!");
            Assert.AreEqual("Global_Error_AlreadyRegistered", result.MessageKey);
        }

        [TestMethod]
        public void InitiateGuestRegistration_Success_ReturnsTrue()
        {
            var user = new user { userId = 1, isGuest = true };
            _mockContext.Setup(c => c.user.Find(1)).Returns(user);
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { user }.AsQueryable()).Object);
            _mockNotification.Setup(n => n.SendVerificationEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            var result = _authCore.InitiateGuestRegistration(1, "new@m.com", "Pass123!");
            Assert.IsTrue(result.Success);
        }

        #endregion

        #region VerifyGuestRegistration

        [TestMethod]
        public void VerifyGuestRegistration_CodeInvalid_ReturnsError()
        {
            var result = _authCore.VerifyGuestRegistration(1, "m@m.com", "bad");
            Assert.AreEqual("Global_Error_CodeInvalid", result.MessageKey);
        }

        [TestMethod]
        public void VerifyGuestRegistration_Success_ReturnsTrue()
        {
            var pending = new pendingRegistration
            {
                email = "real@m.com",
                pin = "123",
                expirationTime = DateTime.UtcNow.AddMinutes(5),
                hashedPassword = "hash"
            };
            var guest = new user { userId = 1, isGuest = true };

            _mockContext.Setup(c => c.pendingRegistration).Returns(DbContextMockFactory.CreateMockDbSet(new List<pendingRegistration> { pending }.AsQueryable()).Object);
            _mockContext.Setup(c => c.user.Find(1)).Returns(guest);

            var result = _authCore.VerifyGuestRegistration(1, "real@m.com", "123");
            Assert.IsTrue(result.Success);
        }

        #endregion

        #region RenewSession

        [TestMethod]
        public void RenewSession_Success_ReturnsToken()
        {
            _mockSession.Setup(s => s.RenewSession("tok")).Returns(true);
            var result = _authCore.RenewSession("tok");
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RenewSession_Failure_ReturnsSessionExpired()
        {
            _mockSession.Setup(s => s.RenewSession("tok")).Returns(false);
            var result = _authCore.RenewSession("tok");
            Assert.AreEqual("Global_Error_SessionExpired", result.MessageKey);
        }

        #endregion
    }
}