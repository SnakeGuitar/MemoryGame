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
        private NotificationService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockMailSender = new Mock<IMailSender>();

            _service = new NotificationService(_mockMailSender.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_ReturnsTrue()
        {
            string email = "test@example.com";
            string pin = "123456";
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()));

            bool result = _service.SendVerificationEmail(email, pin);

            Assert.IsTrue(result, "It should return true if mail delivering was successful.");
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_CallsMailSender()
        {
            string email = "test@example.com";
            string pin = "123456";
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()));

            _service.SendVerificationEmail(email, pin);

            _mockMailSender.Verify(m => m.Send(It.Is<MailMessage>(msg =>
                msg.To.Contains(new MailAddress(email)) &&
                msg.Body.Contains(pin)
            )), Times.Once);
        }

        [TestMethod]
        public void SendVerificationEmail_SmtpException_ReturnsFalse()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new SmtpException("Connection error with Gmail."));

            bool result = _service.SendVerificationEmail("test@example.com", "123456");

            Assert.IsFalse(result, "It should return false if there is an SMTP exception.");
        }

        [TestMethod]
        public void SendVerificationEmail_SmtpException_LogsError()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new SmtpException("Connection error"));

            _service.SendVerificationEmail("test@example.com", "123456");

            _mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
}