using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public static class ValidationHelper
    {
        private const int MAX_EMAIL_LENGTH = 255;
        private const int MIN_PASSWORD_LENGTH = 8;
        private const int MAX_PASSWORD_LENGTH = 100;
        private const int MIN_USERNAME_LENGTH = 3;
        private const int MAX_USERNAME_LENGTH = 30;

        private static readonly Regex EmailRegex = new Regex(
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(250)); // RFC 5322 REGEX
        public static ValidationCode ValidateUsername(string username)
        {

            if (string.IsNullOrEmpty(username))
            {
                return ValidationCode.UsernameEmpty;
            }

            if (username.Length < MIN_USERNAME_LENGTH)
            {
                return ValidationCode.UsernameTooSmall;
            }

            if (username.Length > MAX_USERNAME_LENGTH)
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

            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
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

            if (password.All(ch => char.IsLetterOrDigit(ch)))
            {
                return ValidationCode.PasswordMissingSymbol;
            }

            return ValidationCode.Success;
        }

        public static ValidationCode ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return ValidationCode.EmailEmpty;
            }

            if (email.Length > MAX_EMAIL_LENGTH)
            {
                return ValidationCode.EmailTooLong;
            }

            if (!EmailRegex.IsMatch(email))
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
            UsernameTooSmall,
            UsernameTooLong,
            UsernameInvalidChars,

            PasswordEmpty,
            PasswordLengthInvalid,
            PasswordMissingUpper,
            PasswordMissingLower,
            PasswordMissingDigit,
            PasswordMissingSymbol,

            EmailEmpty,
            EmailInvalidFormat,
            EmailTooLong,

            CodeEmpty,
            CodeLengthInvalid,
            CodeNotNumeric

        }
    }
}
