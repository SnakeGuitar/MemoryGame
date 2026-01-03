using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Validator;
using System;

namespace Test.ValidatorTest
{
    [TestClass]
    public class GameLobbyServiceValidatorTest
    {
        private GameLobbyServiceValidator _validator;

        [TestInitialize]
        public void Setup()
        {
            _validator = new GameLobbyServiceValidator();
        }

        [TestMethod]
        public void IsValidGameCode_SixDigits_ReturnsTrue()
        {
            string code = "123456";
            bool result = _validator.IsValidGameCode(code);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidGameCode_FiveDigits_ReturnsFalse()
        {
            string code = "12345";
            bool result = _validator.IsValidGameCode(code);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidGameCode_ContainsLetters_ReturnsFalse()
        {
            string code = "12A456";
            bool result = _validator.IsValidGameCode(code);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidGameCode_NullString_ReturnsFalse()
        {
            bool result = _validator.IsValidGameCode(null);
            Assert.IsFalse(result);
        }

        // --- IsValidGuestName ---

        [TestMethod]
        public void IsValidGuestName_StandardName_ReturnsTrue()
        {
            bool result = _validator.IsValidGuestName("GuestUser");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidGuestName_EmptyString_ReturnsFalse()
        {
            bool result = _validator.IsValidGuestName("");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidGuestName_OnlySpaces_ReturnsFalse()
        {
            bool result = _validator.IsValidGuestName("   ");
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidGuestName_NameTooLong_ReturnsFalse()
        {
            string longName = new string('A', 31);
            bool result = _validator.IsValidGuestName(longName);
            Assert.IsFalse(result);
        }

        // --- IsValidChatMessage ---

        [TestMethod]
        public void IsValidChatMessage_ValidMessage_ReturnsTrue()
        {
            bool result = _validator.IsValidChatMessage("Hola mundo");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidChatMessage_MessageTooLong_ReturnsFalse()
        {
            string longMsg = new string('a', 501);
            bool result = _validator.IsValidChatMessage(longMsg);
            Assert.IsFalse(result);
        }

        // --- CanJoinLobby ---

        [TestMethod]
        public void CanJoinLobby_LobbyNotFull_ReturnsTrue()
        {
            int currentPlayers = 3;
            bool result = _validator.CanJoinLobby(currentPlayers);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanJoinLobby_LobbyFull_ReturnsFalse()
        {
            int currentPlayers = 4;
            bool result = _validator.CanJoinLobby(currentPlayers);
            Assert.IsFalse(result);
        }

        // --- IsValidCardIndex ---

        [TestMethod]
        public void IsValidCardIndex_WithinRange_ReturnsTrue()
        {
            bool result = _validator.IsValidCardIndex(5, 10);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidCardIndex_NegativeIndex_ReturnsFalse()
        {
            bool result = _validator.IsValidCardIndex(-1, 10);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidCardIndex_IndexOutOfBounds_ReturnsFalse()
        {
            bool result = _validator.IsValidCardIndex(10, 10);
            Assert.IsFalse(result);
        }
    }
}
