using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Shared;
using System.Net.Mail;
using System;

namespace Test.SharedTest
{
    [TestClass]
    public class NotificationServiceTest
    {
        private Mock<IMailSender> _mockMailSender;
        private Mock<ILoggerManager> _mockLogger;
        private NotificationService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockMailSender = new Mock<IMailSender>();
            _mockLogger = new Mock<ILoggerManager>();
            _service = new NotificationService(_mockMailSender.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_ReturnsTrue()
        {
            bool result = _service.SendVerificationEmail("test@example.com", "123456");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_CallsMailSender()
        {
            _service.SendVerificationEmail("test@example.com", "123456");

            _mockMailSender.Verify(m => m.Send(It.IsAny<MailMessage>()), Times.Once);
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_ConstructsCorrectEmailAddress()
        {
            _service.SendVerificationEmail("target@test.com", "123456");

            _mockMailSender.Verify(m => m.Send(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress("target@test.com"))
            )), Times.Once);
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_BodyContainsPin()
        {
            string pin = "999888";

            _service.SendVerificationEmail("target@test.com", pin);

            _mockMailSender.Verify(m => m.Send(It.Is<MailMessage>(msg =>
                msg.Body.Contains(pin)
            )), Times.Once);
        }

        [TestMethod]
        public void SendVerificationEmail_SmtpException_ReturnsFalse()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new SmtpException("Server down"));

            bool result = _service.SendVerificationEmail("test@example.com", "123456");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SendVerificationEmail_SmtpException_LogsError()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new SmtpException("Server down"));

            _service.SendVerificationEmail("test@example.com", "123456");

            _mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
}