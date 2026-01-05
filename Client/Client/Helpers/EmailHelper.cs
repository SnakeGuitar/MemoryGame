using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    internal static class EmailHelper
    {
        public static bool isValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendInvitationEmail(string email, string gameCode)
        {
            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.example.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("sender email", "password app"),
                    EnableSsl = true,
                };

                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("sender email"),
                    Subject = "Game Lobby Invitation",
                    Body = $"You have been invited to join a game lobby! Use the following code to join: {gameCode}",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
