using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Utilities
{
    internal class UserServiceUtilities
    {
        private static Random random = new Random();
        public static string GeneratePin()
        {
            lock (random)
            {
                return random.Next(0, 1000000).ToString("D6");
            }
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool SendVerificationEmail(string email, string pin)
        {
            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("ejemplcrre0@gmail.com", "yyhk mfhg goga lnnt"),
                    EnableSsl = true
                };
                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("email"),
                    Subject = "Memory Game - Verify your email",
                    Body = $"Your verification code is: {pin}",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send verification email. Error: {ex.Message}");
                return false;
            }
        }

        public static bool RemovePendingRegistration(string email)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var pending = db.pendingRegistration
                        .FirstOrDefault(p => p.email == email);
                    if (pending != null)
                    {
                        db.pendingRegistration.Remove(pending);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"removePendingRegistration Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public static string GetUsernameById(int userId)
        {
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var user = db.user.Find(userId);
                    return user?.username;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserNameById Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public static string GenerateGuestPassword()
        {
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digitChars = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+";
            const int minLength = 10;

            var random = new Random();

            var passwordChars = new List<char>
            {
                lowerChars[random.Next(lowerChars.Length)],
                upperChars[random.Next(upperChars.Length)],
                digitChars[random.Next(digitChars.Length)],
                specialChars[random.Next(specialChars.Length)]
            };

            var allChars = lowerChars + upperChars + digitChars + specialChars;
            for (int i = passwordChars.Count; i < minLength; i++)
            {
                passwordChars.Add(allChars[random.Next(allChars.Length)]);
            }

            var shuffledChars = passwordChars.OrderBy(c => random.Next()).ToArray();

            return new string(shuffledChars);
        }
    }
}
