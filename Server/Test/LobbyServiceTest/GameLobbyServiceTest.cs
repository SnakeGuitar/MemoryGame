using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.LobbyService;
using Server.SessionService;
using Server.Shared;
using Server.Validator;

namespace Test.LobbyServiceTest
{
    [TestClass]
    public class GameLobbyServiceTest
    {
        private Mock<ISecurityService> _mockSecurity;
        private Mock<ISessionManager> _mockSession;
        private Mock<ILobbyCallbackProvider> _mockCallbackProvider;
        private Mock<IGameLobbyServiceValidator> _mockValidator;
        private Mock<ILoggerManager> _mockLogger;
        private Mock<IDbContextFactory> _mockDbFactory;

        private GameLobbyService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockSecurity = new Mock<ISecurityService>();
            _mockSession = new Mock<ISessionManager>();
            _mockCallbackProvider = new Mock<ILobbyCallbackProvider>();
            _mockValidator = new Mock<IGameLobbyServiceValidator>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockDbFactory = new Mock<IDbContextFactory>();

            _mockCallbackProvider.Setup(c => c.GetCallback()).Returns(new Mock<IGameLobbyCallback>().Object);

            _service = new GameLobbyService(
                _mockSecurity.Object,
                _mockSession.Object,
                _mockCallbackProvider.Object,
                _mockValidator.Object,
                _mockLogger.Object,
                _mockDbFactory.Object
            );
        }

        [TestMethod]
        public void JoinLobby_InvalidGameCode_ReturnsFalse()
        {
            string invalidCode = "INVALID";
            _mockValidator.Setup(v => v.IsValidGameCode(invalidCode)).Returns(false);

            bool result = _service.JoinLobby("token", invalidCode, false);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void JoinLobby_PlayerNameResolutionFails_ReturnsFalse()
        {
            string validCode = "123456";
            string token = "bad_token";

            _mockValidator.Setup(v => v.IsValidGameCode(validCode)).Returns(true);
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns((int?)null);

            bool result = _service.JoinLobby(token, validCode, false);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void JoinLobby_ValidGuest_ReturnsTrue()
        {
            string validCode = "123456";
            string guestName = "Guest1";
            string hostToken = "HostToken";

            _mockValidator.Setup(v => v.IsValidGameCode(validCode)).Returns(true);
            _mockSession.Setup(s => s.GetUserIdFromToken(hostToken)).Returns(1); // ID del Host
            _mockSecurity.Setup(s => s.GetUsernameById(1)).Returns("HostUser");

            _service.CreateLobby(hostToken, validCode, true);

            _mockValidator.Setup(v => v.IsValidGuestName(guestName)).Returns(true);

            bool result = _service.JoinLobby("any_guest_token", validCode, true, guestName);

            Assert.IsTrue(result, "Debería retornar true al unirse a una sala existente.");
        }

        [TestMethod]
        public void JoinLobby_ValidUser_ReturnsTrue()
        {
            string validCode = "123456";
            string hostToken = "HostToken";
            string playerToken = "PlayerToken";
            int playerId = 10;
            string playerUsername = "GamerPro";

            _mockValidator.Setup(v => v.IsValidGameCode(validCode)).Returns(true);
            _mockSession.Setup(s => s.GetUserIdFromToken(hostToken)).Returns(1);
            _mockSecurity.Setup(s => s.GetUsernameById(1)).Returns("HostUser");

            _service.CreateLobby(hostToken, validCode, true);

            _mockSession.Setup(s => s.GetUserIdFromToken(playerToken)).Returns(playerId);
            _mockSecurity.Setup(sec => sec.GetUsernameById(playerId)).Returns(playerUsername);

            bool result = _service.JoinLobby(playerToken, validCode, false);

            Assert.IsTrue(result, "Debería retornar true al unirse un usuario registrado.");
        }

        [TestMethod]
        public void SendChatMessage_InvalidMessage_LogsWarningOnly()
        {
            string badMsg = "";
            _mockValidator.Setup(v => v.IsValidChatMessage(badMsg)).Returns(false);

            _service.SendChatMessage(badMsg);

            _mockLogger.Verify(l => l.LogWarn(It.Is<string>(s => s.Contains("Invalid chat message"))), Times.Once);
        }

        [TestMethod]
        public void LeaveLobby_NullSession_DoesNotThrow()
        {
            try
            {
                _service.LeaveLobby();
            }
            catch
            {
                Assert.Fail("LeaveLobby should not throw an exception even if session is null.");
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StartGame_NullSession_LogsWarningAndReturnsSafe()
        {
            _service.StartGame(new Server.GameService.GameSettings());

            _mockLogger.Verify(l => l.LogWarn(It.Is<string>(s => s.Contains("StartGame"))), Times.Once);
        }

        [TestMethod]
        public void FlipCard_NullSession_LogsWarningAndReturns()
        {
            _service.FlipCard(0);

            _mockLogger.Verify(l => l.LogWarn(It.Is<string>(s => s.Contains("null session ID"))), Times.Once);
        }

        [TestMethod]
        public void FlipCard_InvalidIndex_DoesNotCallGameManager()
        {
            _mockValidator.Setup(v => v.IsValidCardIndex(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            try
            {
                _service.FlipCard(-1);
            }
            catch
            {
                Assert.Fail("FlipCard should not break under invalid index.");
            }

            Assert.IsTrue(true);
        }
    }
}