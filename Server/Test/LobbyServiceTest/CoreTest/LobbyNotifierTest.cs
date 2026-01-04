using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server.LobbyService;
using Server.LobbyService.Core;
using Server.Shared;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Test.LobbyServiceTest.CoreTest
{
    [TestClass]
    public class LobbyNotifierTest
    {
        private Mock<ILoggerManager> _mockLogger;
        private LobbyNotifier _notifier;
        private Mock<IGameLobbyCallback> _mockCallback;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerManager>();
            _notifier = new LobbyNotifier(_ => { }, _mockLogger.Object);
            _mockCallback = new Mock<IGameLobbyCallback>();
        }

        [TestMethod]
        public void NotifyJoin_ValidLobby_CallsUpdatePlayerList()
        {
            var lobby = new Lobby { GameCode = "123" };
            var client = new LobbyClient { Id = "1", Name = "P1", Callback = _mockCallback.Object };
            lobby.Clients.TryAdd("1", client);

            _notifier.NotifyJoin(lobby, "P1");

            Thread.Sleep(50);

            _mockCallback.Verify(c => c.UpdatePlayerList(It.IsAny<LobbyPlayerInfo[]>()), Times.Once);
        }

        [TestMethod]
        public void NotifyJoin_ValidLobby_CallsReceiveChatMessage()
        {
            var lobby = new Lobby { GameCode = "123" };
            var client = new LobbyClient { Id = "1", Name = "P1", Callback = _mockCallback.Object };
            lobby.Clients.TryAdd("1", client);

            _notifier.NotifyJoin(lobby, "P1");
            Thread.Sleep(50);

            _mockCallback.Verify(c => c.ReceiveChatMessage(It.IsAny<string>(), It.IsAny<string>(), true), Times.Once);
        }

        [TestMethod]
        public void BroadcastMessage_NormalMessage_SendsToClient()
        {
            var lobby = new Lobby { GameCode = "123" };
            var client = new LobbyClient { Id = "1", Name = "P1", Callback = _mockCallback.Object };
            lobby.Clients.TryAdd("1", client);

            _notifier.BroadcastMessage(lobby, "Hello", false, "Sender");
            Thread.Sleep(50);

            _mockCallback.Verify(c => c.ReceiveChatMessage("Sender", "Hello", false), Times.Once);
        }

        [TestMethod]
        public void NotifyLeave_ValidLobby_CallsUpdatePlayerList()
        {
            var lobby = new Lobby { GameCode = "123" };

            var remainingClient = new LobbyClient { Id = "2", Name = "P2", Callback = _mockCallback.Object };
            lobby.Clients.TryAdd("2", remainingClient);

            _notifier.NotifyLeave(lobby, "P1");
            Thread.Sleep(50);

            _mockCallback.Verify(c => c.UpdatePlayerList(It.IsAny<LobbyPlayerInfo[]>()), Times.Once);
        }
    }
}