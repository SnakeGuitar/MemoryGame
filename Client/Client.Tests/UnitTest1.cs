using Client.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static Client.Helpers.ValidationHelper;

namespace Client.Tests
{
    [TestClass]
    public class ValidationHelperTests
    {
        //  USERNAME TESTS

        [TestMethod]
        public void ValidateUsername_ValidStandard_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("GamerOne"));
        }

        [TestMethod]
        public void ValidateUsername_ValidWithAllowedSpecialChars_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("User-Name_123"));
        }

        [TestMethod]
        public void ValidateUsername_ValidMinLength_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("abc"));
        }

        [TestMethod]
        public void ValidateUsername_ValidMaxLength_ReturnsSuccess()
        {
            string maxLen = new string('a', 30);
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername(maxLen));
        }

        [TestMethod]
        public void ValidateUsername_Null_ReturnsUsernameEmpty()
        {
            Assert.AreEqual(ValidationCode.UsernameEmpty, ValidationHelper.ValidateUsername(null));
        }

        [TestMethod]
        public void ValidateUsername_Empty_ReturnsUsernameEmpty()
        {
            Assert.AreEqual(ValidationCode.UsernameEmpty, ValidationHelper.ValidateUsername(""));
        }

        [TestMethod]
        public void ValidateUsername_TooShort_ReturnsUsernameTooSmall()
        {
            var result = ValidationHelper.ValidateUsername("ab");
            Assert.AreEqual(ValidationCode.UsernameTooSmall, result);
        }

        [TestMethod]
        public void ValidateUsername_TooLong_ReturnsUsernameTooLong()
        {
            // Edge case: 31 characters (Limit is 30)
            string tooLong = new string('a', 31);
            Assert.AreEqual(ValidationCode.UsernameTooLong, ValidationHelper.ValidateUsername(tooLong));
        }

        [TestMethod]
        public void ValidateUsername_SpaceChar_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername("User Name"));
        }

        [TestMethod]
        public void ValidateUsername_AtSymbol_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername("User@Name"));
        }

        [TestMethod]
        public void ValidateUsername_SymbolNotAllowed_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername("User#1"));
        }

        [TestMethod]
        public void ValidateUsername_OnlyNumbers_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("123456"));
        }


        // PASSWORD TESTS

        [TestMethod]
        public void ValidatePassword_ValidStandard_ReturnsSuccess()
        {
            // Meets all requirements: >8 chars, Upper, Lower, Digit, Symbol
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword("StrongP@ss1"));
        }

        [TestMethod]
        public void ValidatePassword_ValidMaxLength_ReturnsSuccess()
        {
            // Exact upper limit (100 characters)
            // Repetitive pattern to meet rules and fill space
            string basePass = "Aa1!";
            string longPass = basePass;
            while (longPass.Length < 100) longPass += "a";

            // Final adjustment if it exceeds
            longPass = longPass.Substring(0, 100);

            // Ensure it still meets requirements after trimming
            if (!longPass.Contains("!")) longPass = longPass.Remove(99) + "!";
            if (!longPass.Any(char.IsUpper)) longPass = "A" + longPass.Substring(1);

            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword(longPass));
        }

        [TestMethod]
        public void ValidatePassword_Null_ReturnsPasswordEmpty()
        {
            Assert.AreEqual(ValidationCode.PasswordEmpty, ValidationHelper.ValidatePassword(null));
        }

        [TestMethod]
        public void ValidatePassword_Empty_ReturnsPasswordEmpty()
        {
            Assert.AreEqual(ValidationCode.PasswordEmpty, ValidationHelper.ValidatePassword(""));
        }

        [TestMethod]
        public void ValidatePassword_TooShort_ReturnsLengthInvalid()
        {
            // 7 characters (Limit is 8)
            Assert.AreEqual(ValidationCode.PasswordLengthInvalid, ValidationHelper.ValidatePassword("Aa1!aaa"));
        }

        [TestMethod]
        public void ValidatePassword_TooLong_ReturnsLengthInvalid()
        {
            string tooLong = new string('a', 101);
            Assert.AreEqual(ValidationCode.PasswordLengthInvalid, ValidationHelper.ValidatePassword(tooLong));
        }

        [TestMethod]
        public void ValidatePassword_MissingUpper_ReturnsMissingUpper()
        {
            // Has length, lower, digit, symbol, but NO uppercase
            Assert.AreEqual(ValidationCode.PasswordMissingUpper, ValidationHelper.ValidatePassword("weakp@ss1"));
        }

        [TestMethod]
        public void ValidatePassword_MissingLower_ReturnsMissingLower()
        {
            // Has length, upper, digit, symbol, but NO lowercase
            Assert.AreEqual(ValidationCode.PasswordMissingLower, ValidationHelper.ValidatePassword("WEAKP@SS1"));
        }

        [TestMethod]
        public void ValidatePassword_MissingDigit_ReturnsMissingDigit()
        {
            // Has length, upper, lower, symbol, but NO digit
            Assert.AreEqual(ValidationCode.PasswordMissingDigit, ValidationHelper.ValidatePassword("NoDigit@Pass"));
        }

        [TestMethod]
        public void ValidatePassword_MissingSymbol_ReturnsMissingSymbol()
        {
            // Has length, upper, lower, digit, but ONLY letters and numbers (Missing symbol)
            Assert.AreEqual(ValidationCode.PasswordMissingSymbol, ValidationHelper.ValidatePassword("NoSymbolPass1"));
        }

        [TestMethod]
        public void ValidatePassword_JustLettersAndNumbersLong_ReturnsMissingSymbol()
        {
            // Long case without symbols
            Assert.AreEqual(ValidationCode.PasswordMissingSymbol, ValidationHelper.ValidatePassword("LongPasswordWithoutSymbols123456"));
        }

        [TestMethod]
        public void ValidatePassword_OnlySpaceAsSymbol_ReturnsSuccess()
        {
            // Logic: !All(LetterOrDigit) -> Space is not LetterOrDigit -> Returns Success
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword("Space Pass1"));
        }

        [TestMethod]
        public void ValidatePassword_UnderscoreAsSymbol_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword("Under_Score1"));
        }

        [TestMethod]
        public void ValidatePassword_ComplexSymbols_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword("C#mplex$Pass1"));
        }


        // EMAIL TESTS

        [TestMethod]
        public void ValidateEmail_ValidStandard_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateEmail("test@example.com"));
        }

        [TestMethod]
        public void ValidateEmail_ValidSubdomain_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateEmail("name@sub.domain.co.jp"));
        }

        [TestMethod]
        public void ValidateEmail_Null_ReturnsEmailEmpty()
        {
            Assert.AreEqual(ValidationCode.EmailEmpty, ValidationHelper.ValidateEmail(null));
        }

        [TestMethod]
        public void ValidateEmail_Empty_ReturnsEmailEmpty()
        {
            Assert.AreEqual(ValidationCode.EmailEmpty, ValidationHelper.ValidateEmail(""));
        }

        [TestMethod]
        public void ValidateEmail_TooLong_ReturnsEmailTooLong()
        {
            // Generate email > 255 chars
            string longName = new string('a', 250);
            string longEmail = longName + "@test.com"; // ~260 chars
            Assert.AreEqual(ValidationCode.EmailTooLong, ValidationHelper.ValidateEmail(longEmail));
        }

        [TestMethod]
        public void ValidateEmail_NoAtSymbol_ReturnsInvalidFormat()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("testexample.com"));
        }

        [TestMethod]
        public void ValidateEmail_NoDomain_ReturnsInvalidFormat()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("test@"));
        }

        [TestMethod]
        public void ValidateEmail_NoExtension_ReturnsInvalidFormat()
        {
            // Regex requires a dot: \.[^@\s]+$
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("test@com"));
        }

        [TestMethod]
        public void ValidateEmail_Spaces_ReturnsInvalidFormat()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("test @example.com"));
        }

        [TestMethod]
        public void ValidateEmail_TwoAtSymbols_ReturnsInvalidFormat()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("test@sub@domain.com"));
        }


        // VERIFY CODE TESTS

        [TestMethod]
        public void ValidateVerifyCode_Valid_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateVerifyCode("123456", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Null_ReturnsCodeEmpty()
        {
            Assert.AreEqual(ValidationCode.CodeEmpty, ValidationHelper.ValidateVerifyCode(null, 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Empty_ReturnsCodeEmpty()
        {
            Assert.AreEqual(ValidationCode.CodeEmpty, ValidationHelper.ValidateVerifyCode("", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_TooShort_ReturnsLengthInvalid()
        {
            Assert.AreEqual(ValidationCode.CodeLengthInvalid, ValidationHelper.ValidateVerifyCode("12345", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_TooLong_ReturnsLengthInvalid()
        {
            Assert.AreEqual(ValidationCode.CodeLengthInvalid, ValidationHelper.ValidateVerifyCode("1234567", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_NotNumeric_ReturnsNotNumeric()
        {
            Assert.AreEqual(ValidationCode.CodeNotNumeric, ValidationHelper.ValidateVerifyCode("12345A", 6));
        }
    }
}