using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.GameService;
using Server.LobbyService;
using Server.LobbyService.Core;
using Server.Shared;
using System.Collections.Generic;

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
        public void TryJoinLobby_NewLobby_ReturnsTrue()
        {
            var client = new LobbyClient { Id = "1", SessionId = "s1" };
            bool result = _stateManager.TryJoinLobby("CODE1", client, out _);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryJoinLobby_NewLobby_CreatesLobbyObject()
        {
            var client = new LobbyClient { Id = "1", SessionId = "s1" };
            _stateManager.TryJoinLobby("CODE1", client, out var lobby);

            Assert.IsNotNull(lobby);
        }

        [TestMethod]
        public void TryJoinLobby_LobbyFull_ReturnsFalse()
        {
            string code = "FULL1";
            for (int i = 0; i < 4; i++)
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
            var client = new LobbyClient { Id = "1", SessionId = sessionId };
            _stateManager.TryJoinLobby("CODE", client, out _);

            bool result = _stateManager.IsInLobby(sessionId);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryStartGame_LobbyExists_ReturnsTrue()
        {
            string sessionId = "host_session";
            var client = new LobbyClient { Id = "host", SessionId = sessionId };
            _stateManager.TryJoinLobby("GAME1", client, out _);
            var settings = new GameSettings { CardCount = 16, TurnTimeSeconds = 10 };

            bool result = _stateManager.TryStartGame(sessionId, settings, out _);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TryStartGame_GameAlreadyStarted_ReturnsFalse()
        {
            string sessionId = "host_session";
            _stateManager.TryJoinLobby("GAME1", new LobbyClient { Id = "host", SessionId = sessionId }, out _);
            _stateManager.TryStartGame(sessionId, new GameSettings(), out _);

            bool result = _stateManager.TryStartGame(sessionId, new GameSettings(), out _);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveClient_ClientExists_ReturnsClientObject()
        {
            string sessionId = "remove_me";
            _stateManager.TryJoinLobby("CODE", new LobbyClient { Id = "1", SessionId = sessionId, Name = "Player" }, out _);

            var removedClient = _stateManager.RemoveClient(sessionId, out _);

            Assert.IsNotNull(removedClient);
        }

        [TestMethod]
        public void RemoveClient_ClientDoesNotExist_ReturnsNull()
        {
            var result = _stateManager.RemoveClient("non_existent_session", out _);

            Assert.IsNull(result);
        }
    }
}