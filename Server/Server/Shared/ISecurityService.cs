using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared
{
    public interface ISecurityService
    {
        string GeneratePin();
        string HashPassword(string password);
        bool RemovePendingRegistration(string email);
        string GetUsernameById(int userId);
        string GenerateGuestPassword();
    }

    public class SecurityService : ISecurityService
    {
        private readonly IDbContextFactory _dbFactory;

        public SecurityService(IDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public SecurityService() : this(new DbContextFactory())
        {
        }

        private static readonly Random random = new Random();
        public string GeneratePin()
        {
            lock (random)
            {
                return random.Next(0, 1000000).ToString("D6");
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool RemovePendingRegistration(string email)
        {
            try
            {
                using (var db = _dbFactory.Create())
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

        public string GetUsernameById(int userId)
        {
            try
            {
                using (var db = _dbFactory.Create())
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

        public string GenerateGuestPassword()
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
