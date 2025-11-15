using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public static class ValidationHelper
    {
        public static ValidationCode ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return ValidationCode.UsernameEmpty;
            }

            if (username.Length > 30)
            {
                return ValidationCode.UsernameTooLong;
            }

            if (!username.All(ch => char.IsLetterOrDigit(ch) || ch == '_' || ch == '-'))
            {
                return ValidationCode.UsernameInvalidChars;
            }
                
            return ValidationCode.Success;
        }

        public static ValidationCode ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return ValidationCode.PasswordEmpty;
            }

            if (password.Length < 8 || password.Length > 20)
            {
                return ValidationCode.PasswordLengthInvalid;
            }

            if (!password.Any(char.IsUpper))
            {
                return ValidationCode.PasswordMissingUpper;
            }
                

            if (!password.Any(char.IsLower))
            {
                return ValidationCode.PasswordMissingLower;
            }

            if (!password.Any(char.IsDigit))
            {
                return ValidationCode.PasswordMissingDigit;
            }

            return ValidationCode.Success;
        }

        public static ValidationCode ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return ValidationCode.EmailEmpty;
            }
                
            if (!email.Contains("@") || !email.Contains("."))
            {
                return ValidationCode.EmailInvalidFormat;
            }

            return ValidationCode.Success;
        }

        public static ValidationCode ValidateVerifyCode(string code, int requiredLength)
        {
            if (string.IsNullOrEmpty(code))
            {
                return ValidationCode.CodeEmpty;
            }
            if (code.Length != requiredLength)
            {
                return ValidationCode.CodeLengthInvalid;
            }
            if (!code.All(char.IsDigit))
            {
                return ValidationCode.CodeNotNumeric;
            }
            return ValidationCode.Success;
        }

        public enum ValidationCode
        {
            Success,

            UsernameEmpty,
            UsernameTooLong,
            UsernameInvalidChars,

            PasswordEmpty,
            PasswordLengthInvalid,
            PasswordMissingUpper,
            PasswordMissingLower,
            PasswordMissingDigit,

            EmailEmpty,
            EmailInvalidFormat,

            CodeEmpty,
            CodeLengthInvalid,
            CodeNotNumeric

        }
    }
}
