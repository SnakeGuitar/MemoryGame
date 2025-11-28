using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Server.SessionService;
using Server.Validator;

namespace Test
{
    [TestClass]
    public class UserServiceValidatorTest
    {
        private readonly UserServiceValidator _validator = new UserServiceValidator();

        [DataTestMethod]
        [DataRow("usuario@dominio.com", true)]
        [DataRow("nombre.apellido@empresa.co.uk", true)]
        [DataRow("email+tag@gmail.com", true)]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("   ", false)]
        [DataRow("sinarroba.com", false)]
        [DataRow("email@dominio", false)]
        [DataRow("admin' --@example.com", false)]
        [DataRow("<script>@example.com", false)]
        [DataRow("user@localhost", false)]
        public void IsValidEmail_ReturnsExpected_ForVariousInputs(string email, bool expected)
        {
            bool result = _validator.IsValidEmail(email);
            Assert.AreEqual(expected, result, $"Falló: {email}");
        }

        [TestMethod]
        public void IsValidEmail_BufferOverflowAttack_ReturnsFalse()
        {
            string longEmail = new string('a', 10000) + "@example.com";
            bool result = _validator.IsValidEmail(longEmail);
            Assert.IsFalse(result, "Validation should fail for a long email");
        }

        [DataTestMethod]
        [DataRow("Password123!", true)]
        [DataRow("#StrongPass9", true)]
        [DataRow("short1!", false)]
        [DataRow("alllowercase1!", false)]
        [DataRow("ALLUPPERCASE1!", false)]
        [DataRow("NoNumbers!", false)]
        [DataRow("NoSpecialChar123", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        public void IsValidPassword_ReturnsExpected_ForVariousInputs(string password, bool expected)
        {
            bool result = _validator.IsValidPassword(password);
            Assert.AreEqual(expected, result, $"Falló: {password}");
        }
    }
}
