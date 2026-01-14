using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Server.Validator
{
    public interface IUserServiceValidator
    {
        bool IsValidEmail(string email);
        bool IsValidPassword(string password);
        bool IsValidUsername(string username);
    }
    public class UserServiceValidator : IUserServiceValidator
    {
        private const int MAX_EMAIL_LENGTH = 255;
        private const int MIN_PASSWORD_LENGTH = 8;
        private const int MAX_PASSWORD_LENGTH = 100;
        private const int MIN_USERNAME_LENGTH = 3;
        private const int MAX_USERNAME_LENGTH = 30;

        private static readonly Regex EmailRegex = new Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250));

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (email.Length > MAX_EMAIL_LENGTH)
            {
                return false;
            }

            try
            {
                return EmailRegex.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            
            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
            {
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                return false;
            }
                
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            if (password.All(char.IsLetterOrDigit))
            {
                return false;
            }

            return true;
        }

        public bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return false;
            }

            if (username.Length < MIN_USERNAME_LENGTH || username.Length > MAX_USERNAME_LENGTH)
            {
                return false;
            }

            foreach (char c in username)
            {
                bool isAllowed = char.IsLetterOrDigit(c) || c == '_' || c == '-';
                if (!isAllowed)
                {
                    return false;
                }
            }

            return true;
        }
    }
}