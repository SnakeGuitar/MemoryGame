using System;
using System.Linq;
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

            if (!Utilities.UserServiceValidation.IsValidPassword(password))
            {
                return false; // Invalid password format
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (var db = new memoryGameEntities())
                {

                    db.Database.Connection.Open();
                    db.Database.Connection.Close();

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
                        HashedPassword = hashedPassword,
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RequestRegistration Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw; // Re-throw the exception after logging
            }
        }
        public bool VerifyRegistration(string email, string pin)
        {
            try
            {
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

                    var newUser = new usuario
                    {
                        correo = email,
                        contrasenia = pending.HashedPassword,
                        nombreUsuario = email.Split('@')[0], // Default username from email prefix
                    };

                    db.usuario.Add(newUser);
                    db.SaveChanges();

                    removePendingRegistration(email);
                    db.SaveChanges();
                    return true; // Registration completed successfully
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"VerifyRegistration Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw; // Re-throw the exception after logging
            }
        }

        public bool removePendingRegistration(string email)
        {
            try
            {
                using (var db = new memoryGameEntities())
                {
                    var pending = db.PendingRegistrations
                        .FirstOrDefault(p => p.Email == email);
                    if (pending != null)
                    {
                        db.PendingRegistrations.Remove(pending);
                        db.SaveChanges();
                    }
                    return true; // Successfully removed or no pending registration found
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"removePendingRegistration Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw; // Re-throw the exception after logging
            }
        }
        public bool UpdateUserProfile(string email, string username, byte[] avatar)
        {
            try
            {
                using (var db = new memoryGameEntities())
                {
                    var user = db.usuario.FirstOrDefault(u => u.correo == email);
                    if (user == null)
                    {
                        return false; // User not found
                    }

                    /*
                    if (db.usuario.Any(u => u.nombreUsuario == username && u.correo != email))
                    {
                        return false; // Username already taken
                    }
                    */

                    if (!Utilities.UserServiceValidation.IsValidUsername(username))
                    {
                        return false; // Invalid username format
                    }

                    user.nombreUsuario = username;
                    user.avatar = avatar;
                    db.SaveChanges();
                    return true; // Profile updated successfully
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateUserProfile Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw; // Re-throw the exception after logging
            }
        }

        public bool Login(string email, string password)
        {
            try
            {
                using (var db = new memoryGameEntities())
                {
                    var user = db.usuario.FirstOrDefault(u => u.correo == email);
                    if (user == null)
                    {
                        return false; // User not found
                    }
                    return user != null && BCrypt.Net.BCrypt.Verify(password, user.contrasenia);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw; // Re-throw the exception after logging
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
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool SendVerificationEmail(string email, string pin)
        {
            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("email", "password app"),
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
    }
}
