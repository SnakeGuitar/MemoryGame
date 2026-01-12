using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.GameService;
using Server.LobbyService;
using Server.LobbyService.Core;
using Server.Shared;

namespace Test.LobbyServiceTest.CoreTest
{
    [TestClass]
    public class LobbyStateManagerTest
    {
        private Mock<ILoggerManager> _mockLogger;
        private LobbyStateManager _stateManager;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerManager>();
            _stateManager = new LobbyStateManager(_mockLogger.Object);
        }

        [TestMethod]
        public void TryCreateLobby_NewCode_ReturnsTrue()
        {
            var host = new LobbyClient { Id = "host1", SessionId = "s_host" };
            bool result = _stateManager.TryCreateLobby("NEWCODE", host, out var lobby);

            Assert.IsTrue(result);
            Assert.IsNotNull(lobby);
            Assert.AreEqual("NEWCODE", lobby.GameCode);
        }

        [TestMethod]
        public void TryJoinLobby_LobbyExists_ReturnsTrue()
        {
            var host = new LobbyClient { Id = "host", SessionId = "s_host" };
            _stateManager.TryCreateLobby("CODE1", host, out _);

            var client = new LobbyClient { Id = "1", SessionId = "s1" };
            bool result = _stateManager.TryJoinLobby("CODE1", client, out var lobby);

            Assert.IsTrue(result);
            Assert.IsNotNull(lobby);
            Assert.AreEqual(2, lobby.Clients.Count);
        }

        [TestMethod]
        public void TryJoinLobby_LobbyDoesNotExist_ReturnsFalse()
        {
            var client = new LobbyClient { Id = "1", SessionId = "s1" };

            bool result = _stateManager.TryJoinLobby("NONEXISTENT", client, out var lobby);

            Assert.IsFalse(result);
            Assert.IsNull(lobby);
        }

        [TestMethod]
        public void TryJoinLobby_LobbyFull_ReturnsFalse()
        {
            string code = "FULL1";
            var host = new LobbyClient { Id = "host", SessionId = "s_host" };
            _stateManager.TryCreateLobby(code, host, out _);

            for (int i = 0; i < 3; i++)
            {
                _stateManager.TryJoinLobby(code, new LobbyClient { Id = i.ToString(), SessionId = $"s{i}" }, out _);
            }

            var newClient = new LobbyClient { Id = "5", SessionId = "s5" };
            bool result = _stateManager.TryJoinLobby(code, newClient, out _);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsInLobby_UserJoined_ReturnsTrue()
        {
            string sessionId = "s_test";
            var host = new LobbyClient { Id = "1", SessionId = sessionId };

            _stateManager.TryCreateLobby("CODE", host, out _);

            bool result = _stateManager.IsInLobby(sessionId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryStartGame_LobbyExistsAndIsHost_ReturnsTrue()
        {
            string sessionId = "host_session";
            var client = new LobbyClient { Id = "host", SessionId = sessionId };
            _stateManager.TryCreateLobby("GAME1", client, out _);

            var settings = new GameSettings { CardCount = 16, TurnTimeSeconds = 10 };

            bool result = _stateManager.TryStartGame(sessionId, settings, out _);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryStartGame_GameAlreadyStarted_ReturnsFalse()
        {
            string sessionId = "host_session";
            var client = new LobbyClient { Id = "host", SessionId = sessionId };
            _stateManager.TryCreateLobby("GAME1", client, out _);

            _stateManager.TryStartGame(sessionId, new GameSettings(), out _);

            bool result = _stateManager.TryStartGame(sessionId, new GameSettings(), out _);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveClient_ClientExists_ReturnsClientObject()
        {
            string sessionId = "remove_me";
            var host = new LobbyClient { Id = "1", SessionId = sessionId, Name = "Player" };
            _stateManager.TryCreateLobby("CODE", host, out _);

            var removedClient = _stateManager.RemoveClient(sessionId, out _);

            Assert.IsNotNull(removedClient);
            Assert.AreEqual("Player", removedClient.Name);
        }

        [TestMethod]
        public void RemoveClient_ClientDoesNotExist_ReturnsNull()
        {
            var result = _stateManager.RemoveClient("non_existent_session", out _);

            Assert.IsNull(result);
        }
    }
}