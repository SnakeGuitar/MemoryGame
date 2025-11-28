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
        private ILoggerManager _loggerManager;

        [TestInitialize]
        public void Setup()
        {
            _mockMailSender = new Mock<IMailSender>();
            _service = new NotificationService(_mockMailSender.Object, _loggerManager);
        }

        [TestMethod]
        public void SendVerificationEmail_ValidData_CallsSendAndReturnsTrue()
        {
            string email = "test@example.com";
            string pin = "123456";

            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()));

            bool result = _service.SendVerificationEmail(email, pin);

            Assert.IsTrue(result);

            _mockMailSender.Verify(m => m.Send(It.Is<MailMessage>(msg =>
                msg.To.ToString() == email &&
                msg.Body.Contains(pin)
            )), Times.Once);
        }

        [TestMethod]
        public void SendVerificationEmail_SmtpError_ReturnsFalse()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new SmtpException("Connection error with Gmail"));

            bool result = _service.SendVerificationEmail("test@example.com", "123456");

            Assert.IsFalse(result, "Should return null");
        }
    }
}
