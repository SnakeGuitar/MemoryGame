using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.Shared;
using System;
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
        private SecurityService _securityService;
        private ILoggerManager _logger;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);
            _securityService = new SecurityService(_mockDbFactory.Object, _logger);
        }

        [TestMethod]
        public void GeneratePin_ReturnsNotNull()
        {
            string pin = _securityService.GeneratePin();
            Assert.IsNotNull(pin, "Pin should not be null");
        }

        [TestMethod]
        public void GeneratePin_ReturnsStringWithSixCharacters()
        {
            string pin = _securityService.GeneratePin();
            Assert.AreEqual(6, pin.Length, "PIN should be 6 digits long");
        }

        [TestMethod]
        public void GeneratePin_ReturnsNumericString()
        {
            string pin = _securityService.GeneratePin();
            Assert.IsTrue(int.TryParse(pin, out _), "PIN should be numeric");
        }

        [TestMethod]
        public void HashPassword_ReturnsDifferentStringFromRaw()
        {
            string raw = "super_secret_password_:3";
            string hashed = _securityService.HashPassword(raw);
            Assert.AreNotEqual(raw, hashed);
        }


        [TestMethod]
        public void HashPassword_ReturnsNonEmptyString()
        {
            string raw = "super_secret_password_:3";
            string hashed = _securityService.HashPassword(raw);
            Assert.IsNotNull(hashed);
        }

        [TestMethod]
        public void RemovePendingRegistration_PendingExists_ReturnTrue()
        {
            string email = "very_enterprise_mail@test.com";
            var data = new List<pendingRegistration>
            {
                new pendingRegistration
                {
                    email = email,
                }
            }.AsQueryable();

            var mockSet = DbContextMockFactory.CreateMockDbSet(data);
            _mockContext.Setup(c => c.pendingRegistration).Returns(mockSet.Object);

            bool result = _securityService.RemovePendingRegistration(email);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemovePendingRegistrations_PendingExists_RemovesFromDbSet()
        {
            string email = "very_enterprise_mail@test.com";
            var data = new List<pendingRegistration>
            {
                new pendingRegistration
                {
                    email = email,
                }
            }.AsQueryable();


            var mockSet = DbContextMockFactory.CreateMockDbSet(data);
            _mockContext.Setup(c => c.pendingRegistration).Returns(mockSet.Object);

            _securityService.RemovePendingRegistration(email);

            mockSet.Verify(m => m.Remove(It.Is<pendingRegistration>(p => p.email == email)), Times.Once);
        }

        [TestMethod]
        public void RemovePendingRegistration_PendingExists_SavesChangesToContext()
        {
            string email = "borrar@test.com";
            var data = new List<pendingRegistration>
            {
                new pendingRegistration { email = email }
            }.AsQueryable();

            var mockSet = DbContextMockFactory.CreateMockDbSet(data);
            _mockContext.Setup(c => c.pendingRegistration).Returns(mockSet.Object);

            _securityService.RemovePendingRegistration(email);

            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetUsernameById_UserExists_ReturnsCorrectUsername()
        {
            int userId = 99;
            string expectedName = "waos";
            var data = new List<user>
            {
                new user { userId = userId, username = expectedName }
            }.AsQueryable();

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(data).Object);
            string result = _securityService.GetUsernameById(userId);

            Assert.AreEqual(expectedName, result);
        }
    }
}
