using System.Net.Mail;

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
            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("very enterprise mail :3", "password app");
                smtpClient.EnableSsl = true;

                smtpClient.Send(message);
            }
        }
    }
}
