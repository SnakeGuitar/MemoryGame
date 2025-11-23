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

        public NotificationService(IMailSender mailWrapper)
        {
            _mailWrapper = mailWrapper;
        }

        public NotificationService() : this(new MailWrapper())
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

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send email: {ex.Message}");
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
                    if (stream == null) return null;
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}