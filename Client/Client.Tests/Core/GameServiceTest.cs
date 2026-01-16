using Client.Core;
using Client.GameLobbyServiceReference;
using Client.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace Client.Test.Core
{
    [TestFixture]
    public class GameServiceManagerTests
    {
        [TearDown]
        public void ResetSingleton()
        {
            var field = typeof(GameServiceManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, null);
        }

        #region Helpers
        private GameServiceManager CreateInstanceBypassingConstructor()
        {
            var instance = (GameServiceManager)FormatterServices.GetUninitializedObject(typeof(GameServiceManager));
            var field = typeof(GameServiceManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, instance);
            return instance;
        }

        #endregion

        [Test]
        public void Instance_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var instance = GameServiceManager.Instance;
            });
        }

        [Test]
        public void CreateLobbyAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.CreateLobbyAsync("token", "matchCode", false));
        }

        [Test]
        public void JoinLobbyAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.JoinLobbyAsync("token", "code", false, "user"));
        }

        [Test]
        public void LeaveLobbyAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.LeaveLobbyAsync());
        }

        [Test]
        public void SendInvitationEmailAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.SendInvitationEmailAsync("test@email.com", "Subj", "Body"));
        }

        [Test]
        public void FlipCardAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.FlipCardAsync(1));
        }

        [Test]
        public void SendChatMessageAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.SendChatMessageAsync("hello"));
        }

        [Test]
        public void VoteToKickAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await GameServiceManager.Instance.VoteToKickAsync("PlayerBad"));
        }
        

        #region Callback & Event Tests

        [Test]
        public void PlayerJoined_CallbackInvoked_FiresEventWithCorrectUsername()
        {
            var manager = CreateInstanceBypassingConstructor();
            string joinedPlayer = null;
            manager.PlayerJoined += (player, isGuest) => joinedPlayer = player;

            ((IGameLobbyServiceCallback)manager).PlayerJoined("NewUser", true);

            Assert.That(joinedPlayer, Is.EqualTo("NewUser"));
        }

        [Test]
        public void PlayerLeft_CallbackInvoked_FiresEventWithCorrectUsername()
        {
            var manager = CreateInstanceBypassingConstructor();
            string leftPlayer = null;
            manager.PlayerLeft += (player) => leftPlayer = player;

            ((IGameLobbyServiceCallback)manager).PlayerLeft("Leaver");

            Assert.That(leftPlayer, Is.EqualTo("Leaver"));
        }

        [Test]
        public void PlayerListUpdated_CallbackInvoked_FiresEventWithList()
        {
            var manager = CreateInstanceBypassingConstructor();
            LobbyPlayerInfo[] receivedList = null;
            manager.PlayerListUpdated += (list) => receivedList = list;

            var fakeList = new LobbyPlayerInfo[] { new LobbyPlayerInfo() };
            ((IGameLobbyServiceCallback)manager).UpdatePlayerList(fakeList);

            Assert.That(receivedList, Is.Not.Null);
        }

        [Test]
        public void GameStarted_CallbackInvoked_FiresEventWithBoard()
        {
            var manager = CreateInstanceBypassingConstructor();
            List<CardInfo> receivedBoard = null;
            manager.GameStarted += (board) => receivedBoard = board;

            var fakeBoard = new CardInfo[] { new CardInfo() };
            ((IGameLobbyServiceCallback)manager).GameStarted(fakeBoard);

            Assert.That(receivedBoard, Is.Not.Null);
        }

        [Test]
        public void TurnUpdated_CallbackInvoked_FiresEventWithCorrectPlayer()
        {
            var manager = CreateInstanceBypassingConstructor();
            string turnPlayer = null;
            manager.TurnUpdated += (player, time) => turnPlayer = player;

            ((IGameLobbyServiceCallback)manager).UpdateTurn("CurrentPlayer", 30);

            Assert.That(turnPlayer, Is.EqualTo("CurrentPlayer"));
        }

        [Test]
        public void CardShown_CallbackInvoked_FiresEventWithCorrectIndex()
        {
            var manager = CreateInstanceBypassingConstructor();
            int cardIndex = -1;
            manager.CardShown += (index, img) => cardIndex = index;

            ((IGameLobbyServiceCallback)manager).ShowCard(5, "img_1");

            Assert.That(cardIndex, Is.EqualTo(5));
        }

        [Test]
        public void CardsHidden_CallbackInvoked_FiresEventWithCorrectFirstIndex()
        {
            var manager = CreateInstanceBypassingConstructor();
            int card1 = -1;
            manager.CardsHidden += (c1, c2) => card1 = c1;

            ((IGameLobbyServiceCallback)manager).HideCards(10, 20);

            Assert.That(card1, Is.EqualTo(10));
        }

        [Test]
        public void CardsMatched_CallbackInvoked_FiresEventWithCorrectSecondIndex()
        {
            var manager = CreateInstanceBypassingConstructor();
            int card2 = -1;
            manager.CardsMatched += (c1, c2) => card2 = c2;

            ((IGameLobbyServiceCallback)manager).SetCardsAsMatched(1, 2);

            Assert.That(card2, Is.EqualTo(2));
        }

        [Test]
        public void ScoreUpdated_CallbackInvoked_FiresEventWithCorrectScore()
        {
            var manager = CreateInstanceBypassingConstructor();
            int score = -1;
            manager.ScoreUpdated += (player, s) => score = s;

            ((IGameLobbyServiceCallback)manager).UpdateScore("Player1", 100);

            Assert.That(score, Is.EqualTo(100));
        }

        [Test]
        public void GameFinished_CallbackInvoked_FiresEventWithWinnerName()
        {
            var manager = CreateInstanceBypassingConstructor();
            string winner = null;
            manager.GameFinished += (w) => winner = w;

            ((IGameLobbyServiceCallback)manager).GameFinished("WinnerUser");

            Assert.That(winner, Is.EqualTo("WinnerUser"));
        }

        [Test]
        public void ServerConnectionLost_CallbackInvoked_FiresEvent()
        {
            var manager = CreateInstanceBypassingConstructor();
            bool eventFired = false;
            manager.ServerConnectionLost += () => eventFired = true;
            Assert.Pass();
        }

        #endregion
    }
}