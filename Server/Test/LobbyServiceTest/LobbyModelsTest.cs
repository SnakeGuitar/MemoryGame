using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.LobbyService;
using System;

namespace Test.LobbyServiceTest
{
    [TestClass]
    public class LobbyModelsTest
    {
        [TestMethod]
        public void Lobby_Constructor_InitializesClientsDictionary()
        {
            var lobby = new Lobby();

            Assert.IsNotNull(lobby.Clients, "Client collection should not be null.");
        }

        [TestMethod]
        public void Lobby_Constructor_InitializesLockObject()
        {
            var lobby = new Lobby();

            Assert.IsNotNull(lobby.LockObject, "LockObject should not be null.");
        }

        [TestMethod]
        public void Lobby_Properties_CanSetAndGetGameCode()
        {
            var lobby = new Lobby();
            string expected = "CODE123";

            lobby.GameCode = expected;

            Assert.AreEqual(expected, lobby.GameCode);
        }

        [TestMethod]
        public void LobbyClient_Properties_CanSetAndGetValues()
        {
            var client = new LobbyClient();
            string id = "User1";

            client.Id = id;

            Assert.AreEqual(id, client.Id);
        }
    }
}