using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.Shared;
using System.Collections.Generic;
using System.Linq;
using Test.Helpers;

namespace Test.SharedTest
{
    [TestClass]
    public class SecurityServiceTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<ILoggerManager> _mockLogger;
        private SecurityService _securityService;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _securityService = new SecurityService(_mockDbFactory.Object, _mockLogger.Object);
        }

        // --- GeneratePin Tests ---

        [TestMethod]
        public void GeneratePin_ReturnsNotNull()
        {
            string pin = _securityService.GeneratePin();
            Assert.IsNotNull(pin);
        }

        [TestMethod]
        public void GeneratePin_ReturnsStringWithSixCharacters()
        {
            string pin = _securityService.GeneratePin();
            Assert.AreEqual(6, pin.Length);
        }

        [TestMethod]
        public void GeneratePin_ReturnsNumericString()
        {
            string pin = _securityService.GeneratePin();
            Assert.IsTrue(int.TryParse(pin, out _));
        }

        // --- HashPassword Tests ---

        [TestMethod]
        public void HashPassword_ReturnsHashedString()
        {
            string raw = "password";
            string hashed = _securityService.HashPassword(raw);
            Assert.AreNotEqual(raw, hashed);
        }

        [TestMethod]
        public void HashPassword_ReturnsNonEmptyString()
        {
            string hashed = _securityService.HashPassword("password");
            Assert.IsFalse(string.IsNullOrEmpty(hashed));
        }

        // --- GenerateGuestPassword Tests ---

        [TestMethod]
        public void GenerateGuestPassword_ReturnsStringWithAtLeastTenChars()
        {
            string pass = _securityService.GenerateGuestPassword();
            Assert.IsTrue(pass.Length >= 10);
        }

        [TestMethod]
        public void GenerateGuestPassword_ContainsDigit()
        {
            string pass = _securityService.GenerateGuestPassword();
            Assert.IsTrue(pass.Any(char.IsDigit));
        }

        [TestMethod]
        public void GenerateGuestPassword_ContainsUpperCase()
        {
            string pass = _securityService.GenerateGuestPassword();
            Assert.IsTrue(pass.Any(char.IsUpper));
        }

        // --- RemovePendingRegistration Tests ---

        [TestMethod]
        public void RemovePendingRegistration_EmailExists_ReturnsTrue()
        {
            string email = "test@test.com";
            var data = new List<pendingRegistration> { new pendingRegistration { email = email } }.AsQueryable();
            var mockSet = DbContextMockFactory.CreateMockDbSet(data);
            _mockContext.Setup(c => c.pendingRegistration).Returns(mockSet.Object);

            bool result = _securityService.RemovePendingRegistration(email);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemovePendingRegistration_EmailExists_CallsRemove()
        {
            string email = "test@test.com";
            var data = new List<pendingRegistration> { new pendingRegistration { email = email } }.AsQueryable();
            var mockSet = DbContextMockFactory.CreateMockDbSet(data);
            _mockContext.Setup(c => c.pendingRegistration).Returns(mockSet.Object);

            _securityService.RemovePendingRegistration(email);

            mockSet.Verify(m => m.Remove(It.IsAny<pendingRegistration>()), Times.Once);
        }

        [TestMethod]
        public void RemovePendingRegistration_DbError_ReturnsFalse()
        {
            _mockContext.Setup(c => c.pendingRegistration).Throws(new System.Exception("DB Error"));

            bool result = _securityService.RemovePendingRegistration("any@email.com");

            Assert.IsFalse(result);
        }

        // --- GetUsernameById Tests ---

        [TestMethod]
        public void GetUsernameById_UserExists_ReturnsUsername()
        {
            int id = 1;
            string expected = "PlayerOne";
            var user = new user { userId = id, username = expected };

            _mockContext.Setup(c => c.user.Find(id)).Returns(user);

            string result = _securityService.GetUsernameById(id);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetUsernameById_UserNotFound_ReturnsNull()
        {
            int id = 1;
            _mockContext.Setup(c => c.user.Find(id)).Returns((user)null);

            string result = _securityService.GetUsernameById(id);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUsernameById_DbError_ReturnsNull()
        {
            _mockContext.Setup(c => c.user).Throws(new System.Exception());

            string result = _securityService.GetUsernameById(1);

            Assert.IsNull(result);
        }
    }
}