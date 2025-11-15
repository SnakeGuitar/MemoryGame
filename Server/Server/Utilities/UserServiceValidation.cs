using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utilities
{
    internal class UserServiceValidation
    {
        public static bool IsValidPassword(string password)
        {
            // Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character
            if (password.Length < 8)
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

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return false;
            }

            return true;
        }

        public static bool IsValidUsername(string username)
        {
            // Username must be less than 30 characters long and contain only letters, digits, underscores, or hyphens
            if (username.Length > 30)
            {
                return false;
            }

            if (!username.All(ch => char.IsLetterOrDigit(ch) || ch == '_' || ch == '-'))
            {
                return false;
            }

            return true;
        }
    }
}
