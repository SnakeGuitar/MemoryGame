using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.Shared;
using System;
using System.Net.Mail;

namespace Test.SharedTest
{
    [TestClass]
    public class NotificationServiceTest
    {
        private NotificationService _notificationService;
        private Mock<IMailSender> _mockMailSender;
        private Mock<ILoggerManager> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockMailSender = new Mock<IMailSender>();
            _mockLogger = new Mock<ILoggerManager>();

            _notificationService = new NotificationService(_mockMailSender.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void SendVerificationEmail_Success_ReturnsTrue()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()));

            var result = _notificationService.SendVerificationEmail("test@email.com", "123456");

            Assert.IsTrue(result);
            _mockMailSender.Verify(m => m.Send(It.IsAny<MailMessage>()), Times.Once);

            _mockLogger.Verify(l => l.LogInfo(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void SendVerificationEmail_SmtpException_ReturnsFalse()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new SmtpException("Connection failed"));

            var result = _notificationService.SendVerificationEmail("test@email.com", "123456");

            Assert.IsFalse(result);

            _mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestMethod]
        public void SendVerificationEmail_GeneralException_ReturnsFalse()
        {
            _mockMailSender.Setup(m => m.Send(It.IsAny<MailMessage>()))
                           .Throws(new Exception("Unknown error"));

            var result = _notificationService.SendVerificationEmail("test@email.com", "123456");

            Assert.IsFalse(result);

            _mockLogger.Verify(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
}