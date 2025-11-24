using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared
{
    public interface INotificationService
    {
        bool SendVerificationEmail(string email, string pin);
    }

    public class NotificationService : INotificationService
    {
        private readonly IMailSender _mailWrapper;
        private readonly ILoggerManager _logger;
        public NotificationService(IMailSender mailWrapper, ILoggerManager logger)
        {
            _mailWrapper = mailWrapper;
            _logger = logger;
        }

        public NotificationService() : this(
            new MailWrapper(),
            new Logger(typeof(NotificationService)))
        {
        }

        public bool SendVerificationEmail(string email, string pin)
        {
            try
            {
                string body = LoadEmailTemplate("VerificationEmail.html");

                if (string.IsNullOrEmpty(body))
                {
                    body = $"Your verification code is: {pin}";
                }
                else
                {
                    body = body.Replace("{{PIN}}", pin);
                }

                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("very enterprise mail :3", "Memory Game Support"),
                    Subject = "Memory Game - Verify your email",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);
                _mailWrapper.Send(mailMessage);

                _logger.LogInfo($"Sent verification email to {email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send verification email to {email}: {ex.Message}");
                return false;
            }
        }
        private string LoadEmailTemplate(string resourceName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                string resourcePath = $"Server.Resources.{resourceName}";

                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                {
                    if (stream == null)
                    {
                        _logger.LogWarn($"Email template resource '{resourcePath}' not found.");
                        return null;
                    }
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        _logger.LogInfo($"Loaded email template '{resourceName}' successfully.");
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                _logger.LogError($"Error loading email template '{resourceName}'.");
                return null;
            }
        }
    }
}