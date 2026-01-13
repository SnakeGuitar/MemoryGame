using Client.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using static Client.Helpers.ValidationHelper;

namespace Client.Test.Helpers
{
    [TestClass]
    public class ValidationHelperTests
    {
        #region Username Tests

        [TestMethod]
        public void ValidateUsername_Boundary_MinMinus1_ReturnsTooSmall()
        {
            Assert.AreEqual(ValidationCode.UsernameTooSmall, ValidationHelper.ValidateUsername("ab"));
        }

        [TestMethod]
        public void ValidateUsername_Boundary_Min_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("abc"));
        }

        [TestMethod]
        public void ValidateUsername_Boundary_MinPlus1_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("abcd"));
        }

        [TestMethod]
        public void ValidateUsername_Boundary_MaxMinus1_ReturnsSuccess()
        {
            string username = new string('a', 29);
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername(username));
        }

        [TestMethod]
        public void ValidateUsername_Boundary_Max_ReturnsSuccess()
        {
            string username = new string('a', 30);
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername(username));
        }

        [TestMethod]
        public void ValidateUsername_Boundary_MaxPlus1_ReturnsTooLong()
        {
            string username = new string('a', 31);
            Assert.AreEqual(ValidationCode.UsernameTooLong, ValidationHelper.ValidateUsername(username));
        }

        [TestMethod]
        public void ValidateUsername_Chars_Alphanumeric_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("User123"));
        }

        [TestMethod]
        public void ValidateUsername_Chars_UnderscoreOnly_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("___"));
        }

        [TestMethod]
        public void ValidateUsername_Chars_HyphenOnly_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("---"));
        }

        [TestMethod]
        public void ValidateUsername_Chars_MixedAllowedSpecial_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateUsername("A-B_C"));
        }

        [TestMethod]
        public void ValidateUsername_Chars_SpaceMiddle_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername("A B"));
        }

        [TestMethod]
        public void ValidateUsername_Chars_SpaceStart_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername(" ABC"));
        }

        [TestMethod]
        public void ValidateUsername_Chars_SpaceEnd_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername("ABC "));
        }

        [TestMethod]
        public void ValidateUsername_Chars_SymbolAt_ReturnsInvalidChars()
        {
            Assert.AreEqual(ValidationCode.UsernameInvalidChars, ValidationHelper.ValidateUsername("User@1"));
        }

        [TestMethod]
        public void ValidateUsername_Null_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.UsernameEmpty, ValidationHelper.ValidateUsername(null));
        }

        [TestMethod]
        public void ValidateUsername_EmptyString_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.UsernameEmpty, ValidationHelper.ValidateUsername(""));
        }

        #endregion

        #region Password Tests

        [TestMethod]
        public void ValidatePassword_Boundary_MinMinus1_ReturnsLengthInvalid()
        {
            Assert.AreEqual(ValidationCode.PasswordLengthInvalid, ValidationHelper.ValidatePassword("Aa1!aaa"));
        }

        [TestMethod]
        public void ValidatePassword_Boundary_Min_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword("Aa1!aaaa"));
        }

        [TestMethod]
        public void ValidatePassword_Boundary_MinPlus1_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword("Aa1!aaaaa"));
        }

        [TestMethod]
        public void ValidatePassword_Boundary_MaxMinus1_ReturnsSuccess()
        {
            string pass = "Aa1!" + new string('a', 95);
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword(pass));
        }

        [TestMethod]
        public void ValidatePassword_Boundary_Max_ReturnsSuccess()
        {
            string pass = "Aa1!" + new string('a', 96);
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidatePassword(pass));
        }

        [TestMethod]
        public void ValidatePassword_Boundary_MaxPlus1_ReturnsLengthInvalid()
        {
            string pass = "Aa1!" + new string('a', 97);
            Assert.AreEqual(ValidationCode.PasswordLengthInvalid, ValidationHelper.ValidatePassword(pass));
        }

        [TestMethod]
        public void ValidatePassword_MissingUpper_ReturnsError()
        {
            Assert.AreEqual(ValidationCode.PasswordMissingUpper, ValidationHelper.ValidatePassword("aa1!aaaa"));
        }

        [TestMethod]
        public void ValidatePassword_MissingLower_ReturnsError()
        {
            Assert.AreEqual(ValidationCode.PasswordMissingLower, ValidationHelper.ValidatePassword("AA1!AAAA"));
        }

        [TestMethod]
        public void ValidatePassword_MissingDigit_ReturnsError()
        {
            Assert.AreEqual(ValidationCode.PasswordMissingDigit, ValidationHelper.ValidatePassword("Aa!!aaaa"));
        }

        [TestMethod]
        public void ValidatePassword_MissingSymbol_ReturnsError()
        {
            Assert.AreEqual(ValidationCode.PasswordMissingSymbol, ValidationHelper.ValidatePassword("Aa11aaaa"));
        }

        [TestMethod]
        public void ValidatePassword_Null_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.PasswordEmpty, ValidationHelper.ValidatePassword(null));
        }

        [TestMethod]
        public void ValidatePassword_Empty_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.PasswordEmpty, ValidationHelper.ValidatePassword(""));
        }

        #endregion

        #region Email Tests

        [TestMethod]
        public void ValidateEmail_Boundary_Max_ReturnsSuccess()
        {
            string domain = "@test.com"; 
            string localPart = new string('a', 255 - domain.Length);
            string email = localPart + domain;

            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateEmail(email));
        }

        [TestMethod]
        public void ValidateEmail_Boundary_MaxPlus1_ReturnsTooLong()
        {
            string domain = "@test.com";
            string localPart = new string('a', 256 - domain.Length);
            string email = localPart + domain;

            Assert.AreEqual(ValidationCode.EmailTooLong, ValidationHelper.ValidateEmail(email));
        }

        [TestMethod]
        public void ValidateEmail_Format_Simple_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateEmail("a@b.com"));
        }

        [TestMethod]
        public void ValidateEmail_Format_Subdomain_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateEmail("user@mail.server.com"));
        }

        [TestMethod]
        public void ValidateEmail_Format_NoAt_ReturnsInvalid()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("userdomain.com"));
        }

        [TestMethod]
        public void ValidateEmail_Format_NoDomain_ReturnsInvalid()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("user@"));
        }

        [TestMethod]
        public void ValidateEmail_Format_NoLocalPart_ReturnsInvalid()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("@domain.com"));
        }

        [TestMethod]
        public void ValidateEmail_Format_DoubleAt_ReturnsInvalid()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("user@domain@com"));
        }

        [TestMethod]
        public void ValidateEmail_Format_SpaceInLocal_ReturnsInvalid()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("user name@domain.com"));
        }

        [TestMethod]
        public void ValidateEmail_Format_SpaceInDomain_ReturnsInvalid()
        {
            Assert.AreEqual(ValidationCode.EmailInvalidFormat, ValidationHelper.ValidateEmail("user@domain .com"));
        }

        [TestMethod]
        public void ValidateEmail_Null_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.EmailEmpty, ValidationHelper.ValidateEmail(null));
        }

        [TestMethod]
        public void ValidateEmail_Empty_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.EmailEmpty, ValidationHelper.ValidateEmail(""));
        }

        #endregion

        #region VerifyCode Tests

        [TestMethod]
        public void ValidateVerifyCode_Boundary_LenMinus1_ReturnsLengthInvalid()
        {
            Assert.AreEqual(ValidationCode.CodeLengthInvalid, ValidationHelper.ValidateVerifyCode("12345", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Boundary_ExactLen_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateVerifyCode("123456", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Boundary_LenPlus1_ReturnsLengthInvalid()
        {
            Assert.AreEqual(ValidationCode.CodeLengthInvalid, ValidationHelper.ValidateVerifyCode("1234567", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Content_Numeric_ReturnsSuccess()
        {
            Assert.AreEqual(ValidationCode.Success, ValidationHelper.ValidateVerifyCode("000000", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Content_Alpha_ReturnsNotNumeric()
        {
            Assert.AreEqual(ValidationCode.CodeNotNumeric, ValidationHelper.ValidateVerifyCode("ABCDEF", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Content_AlphaNumeric_ReturnsNotNumeric()
        {
            Assert.AreEqual(ValidationCode.CodeNotNumeric, ValidationHelper.ValidateVerifyCode("123A56", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Content_Symbols_ReturnsNotNumeric()
        {
            Assert.AreEqual(ValidationCode.CodeNotNumeric, ValidationHelper.ValidateVerifyCode("12-345", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Content_NegativeSign_ReturnsNotNumeric()
        {
            Assert.AreEqual(ValidationCode.CodeNotNumeric, ValidationHelper.ValidateVerifyCode("-12345", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Content_DecimalPoint_ReturnsNotNumeric()
        {
            Assert.AreEqual(ValidationCode.CodeNotNumeric, ValidationHelper.ValidateVerifyCode("12.345", 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Null_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.CodeEmpty, ValidationHelper.ValidateVerifyCode(null, 6));
        }

        [TestMethod]
        public void ValidateVerifyCode_Empty_ReturnsEmpty()
        {
            Assert.AreEqual(ValidationCode.CodeEmpty, ValidationHelper.ValidateVerifyCode("", 6));
        }

        #endregion
    }
}