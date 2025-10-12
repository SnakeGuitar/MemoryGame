using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

// DATABASE VARIABLES ARE IN SPANISH
// CHANGE TO ENGLISH LATER

// BASE DATOS ESTÁ EN ESPAÑOL
// CAMBIAR A INGLÉS MÁS TARDE

namespace Server.AuthenticationService
{
    internal class UserService : IUserService
    {
        private const int PIN_LENGTH = 6;
        private static readonly Random random = new Random();
        public bool RequestRegistration(string email, string password)
        {

            if (!Utilities.UserServiceValidation.IsValidEmail(email) || 
                !Utilities.UserServiceValidation.IsValidPassword(password))
            {
                return false; // Invalid email or password format
            }

            using (var db = new memoryGameEntities())
            {
                if (db.usuario.Any(u => u.correo == email))
                {
                    return false; // User already exists
                }

                var existingPending = db.PendingRegistrations
                    .FirstOrDefault(p => p.Email == email && p.ExpiryTime > DateTime.Now);
                if (existingPending != null)
                {
                    db.PendingRegistrations.Remove(existingPending); // Remove existing pending registration
                }
                string pin = GeneratePin();
                var pendingRegistration = new PendingRegistrations
                {
                    Email = email,
                    Pin = pin,
                    ExpiryTime = DateTime.Now.AddMinutes(15), // PIN valid for 15 minutes
                    CreatedAt = DateTime.Now
                };

                db.PendingRegistrations.Add(pendingRegistration);
                db.SaveChanges();

                if (!SendVerificationEmail(email, pin))
                {
                    db.PendingRegistrations.Remove(pendingRegistration);
                    db.SaveChanges();
                    return false; // Failed to send email
                }
                return true;
            }
        }

        public bool VerifyRegistration(string email, string pin)
        {
            using (var db = new memoryGameEntities())
            {
                var pendingRegistration = db.PendingRegistrations
                    .FirstOrDefault(p => p.Email == email &&
                    p.Pin == pin &&
                    p.ExpiryTime > DateTime.Now);

                if (pendingRegistration == null)
                {
                    return false; // Invalid or expired PIN
                }

                // Move user from PendingRegistrations to usuario table
                throw new NotImplementedException("Moving user from PendingRegistrations to usuario table is not implemented.");
            }
        }

        public bool CompleteRegistration(string email, string pin, string password)
        {
            if (!Utilities.UserServiceValidation.IsValidPassword(password))
            {
                return false; // Invalid password format
            }

            using (var db = new memoryGameEntities())
            {
                var pending = db.PendingRegistrations
                    .FirstOrDefault(p => p.Email == email &&
                    p.Pin == pin &&
                    p.ExpiryTime > DateTime.Now);

                if (pending == null)
                {
                    return false; // Invalid or expired PIN
                }

                if (db.usuario.Any(u => u.correo == email))
                {
                    return false; // User already exists
                }

                string hashedPassword = HashPassword(password);
                var newUser = new usuario
                {
                    correo = email,
                    contrasenia = hashedPassword,
                    nombreUsuario = email.Split('@')[0], // Default username from email prefix
                };
                db.usuario.Add(newUser);
                db.PendingRegistrations.Remove(pending);
                db.SaveChanges();
            }
            return true; // Registration completed successfully
        }
        public bool Login(string email, string password)
        {
            using (var db = new memoryGameEntities())
            {
                string hashedPassword = HashPassword(password);
                var user = db.usuario.FirstOrDefault(u => u.correo == email && u.contrasenia == hashedPassword);
                return user != null; // Return true if user is found, otherwise false
            }
        }

        private string GeneratePin()
        {
            lock (random)
            {
                return random.Next(0, 1000000).ToString("D6");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        private bool SendVerificationEmail(string email, string pin)
        {
            try
            {
            var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("mail", "password app"),
                EnableSsl = true
            };
            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress("sender mail"),
                Subject = "Memory Game - Verify your email",
                Body = $"Your verification code is: {pin}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
            return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
