using System.Net.Mail;
using System.Configuration;

namespace Server.Shared
{
    public interface IMailSender
    {
        void Send(MailMessage message);
    }

    public class MailWrapper : IMailSender
    {
        public void Send(MailMessage message)
        {
            string host = ConfigurationManager.AppSettings["EmailHost"];
            int port = int.Parse(ConfigurationManager.AppSettings["EmailPort"]);
            string email = ConfigurationManager.AppSettings["EmailSender"];
            string password = ConfigurationManager.AppSettings["EmailPassword"];

            using (var smtpClient = new SmtpClient(host))
            {
                smtpClient.Port = port;
                smtpClient.Credentials = new System.Net.NetworkCredential(email, password);
                smtpClient.EnableSsl = true;

                smtpClient.Send(message);
            }
        }
    }
}