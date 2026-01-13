using Client.Core;
using Client.GameLobbyServiceReference;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace Client.Test.Core
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class GameServiceManagerTests
    {
        [SetUp]
        public void Setup()
        {
            ResetSingleton();
        }

        [TearDown]
        public void TearDown()
        {
            ResetSingleton();
        }

        private void ResetSingleton()
        {
            var field = typeof(GameServiceManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(null, null);
            }
        }

        #region Singleton Tests

        [Test]
        public void Instance_IsNotNull()
        {
            var instance = GameServiceManager.Instance;
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Instance_AlwaysReturnsSameObject()
        {
            var instance1 = GameServiceManager.Instance;
            var instance2 = GameServiceManager.Instance;

            Assert.That(instance1, Is.SameAs(instance2));
        }

        #endregion

        #region Callback Event Tests


        [Test]
        public void ReceiveChatMessage_PassesCorrectSender()
        {
            string receivedSender = null;
            GameServiceManager.Instance.ChatMessageReceived += (sender, msg, isNotif) => receivedSender = sender;

            GameServiceManager.Instance.ReceiveChatMessage("User1", "Hello", false);

            Assert.That(receivedSender, Is.EqualTo("User1"));
        }

        [Test]
        public void ReceiveChatMessage_PassesCorrectMessage()
        {
            string receivedMsg = null;
            GameServiceManager.Instance.ChatMessageReceived += (sender, msg, isNotif) => receivedMsg = msg;

            GameServiceManager.Instance.ReceiveChatMessage("User1", "Hello", false);

            Assert.That(receivedMsg, Is.EqualTo("Hello"));
        }


        [Test]
        public void PlayerJoined_InvokesEventWithCorrectName()
        {
            string joinedPlayer = null;
            GameServiceManager.Instance.PlayerJoined += (player, isGuest) => joinedPlayer = player;

            ((IGameLobbyServiceCallback)GameServiceManager.Instance).PlayerJoined("NewPlayer", false);

            Assert.That(joinedPlayer, Is.EqualTo("NewPlayer"));
        }


        [Test]
        public void PlayerLeft_InvokesEventWithCorrectName()
        {
            string leftPlayer = null;
            GameServiceManager.Instance.PlayerLeft += (player) => leftPlayer = player;

            ((IGameLobbyServiceCallback)GameServiceManager.Instance).PlayerLeft("OldPlayer");

            Assert.That(leftPlayer, Is.EqualTo("OldPlayer"));
        }


        [Test]
        public void UpdatePlayerList_EventReceivesNotNullList()
        {
            LobbyPlayerInfo[] receivedList = null;
            GameServiceManager.Instance.PlayerListUpdated += (list) => receivedList = list;

            var fakeList = new LobbyPlayerInfo[] { new LobbyPlayerInfo { Name = "P1" } };
            GameServiceManager.Instance.UpdatePlayerList(fakeList);

            Assert.That(receivedList, Is.Not.Null);
        }

        [Test]
        public void UpdatePlayerList_EventReceivesListWithCorrectCount()
        {
            LobbyPlayerInfo[] receivedList = null;
            GameServiceManager.Instance.PlayerListUpdated += (list) => receivedList = list;

            var fakeList = new LobbyPlayerInfo[] { new LobbyPlayerInfo { Name = "P1" } };
            GameServiceManager.Instance.UpdatePlayerList(fakeList);

            Assert.That(receivedList.Length, Is.EqualTo(1));
        }

        [Test]
        public void UpdatePlayerList_EventReceivesCorrectData()
        {
            LobbyPlayerInfo[] receivedList = null;
            GameServiceManager.Instance.PlayerListUpdated += (list) => receivedList = list;

            var fakeList = new LobbyPlayerInfo[] {
                new LobbyPlayerInfo { Name = "P1", JoinedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            };
            GameServiceManager.Instance.UpdatePlayerList(fakeList);

            Assert.That(receivedList[0].Name, Is.EqualTo("P1"));
        }

        [Test]
        public void GameStarted_EventReceivesNotNullBoard()
        {
            List<CardInfo> receivedBoard = null;
            GameServiceManager.Instance.GameStarted += (board) => receivedBoard = board;

            var fakeBoard = new CardInfo[] { new CardInfo { CardId = 1 } };
            ((IGameLobbyServiceCallback)GameServiceManager.Instance).GameStarted(fakeBoard);

            Assert.That(receivedBoard, Is.Not.Null);
        }

        [Test]
        public void GameStarted_EventReceivesBoardWithCorrectCount()
        {
            List<CardInfo> receivedBoard = null;
            GameServiceManager.Instance.GameStarted += (board) => receivedBoard = board;

            var fakeBoard = new CardInfo[] { new CardInfo { CardId = 1 } };
            ((IGameLobbyServiceCallback)GameServiceManager.Instance).GameStarted(fakeBoard);

            Assert.That(receivedBoard.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpdateTurn_InvokesEventWithCorrectPlayer()
        {
            string turnPlayer = null;
            GameServiceManager.Instance.TurnUpdated += (player, time) => turnPlayer = player;

            GameServiceManager.Instance.UpdateTurn("CurrentPlayer", 30);

            Assert.That(turnPlayer, Is.EqualTo("CurrentPlayer"));
        }

        [Test]
        public void ShowCard_InvokesEventWithCorrectIndex()
        {
            int cardIdx = -1;
            GameServiceManager.Instance.CardShown += (idx, img) => cardIdx = idx;

            GameServiceManager.Instance.ShowCard(5, "image_rabbit");

            Assert.That(cardIdx, Is.EqualTo(5));
        }

        [Test]
        public void ShowCard_InvokesEventWithCorrectImage()
        {
            string imgId = null;
            GameServiceManager.Instance.CardShown += (idx, img) => imgId = img;

            GameServiceManager.Instance.ShowCard(5, "image_rabbit");

            Assert.That(imgId, Is.EqualTo("image_rabbit"));
        }

        [Test]
        public void HideCards_InvokesEventWithCorrectIndex1()
        {
            int c1 = -1;
            GameServiceManager.Instance.CardsHidden += (idx1, idx2) => c1 = idx1;

            GameServiceManager.Instance.HideCards(1, 2);

            Assert.That(c1, Is.EqualTo(1));
        }

        [Test]
        public void HideCards_InvokesEventWithCorrectIndex2()
        {
            int c2 = -1;
            GameServiceManager.Instance.CardsHidden += (idx1, idx2) => c2 = idx2;

            GameServiceManager.Instance.HideCards(1, 2);

            Assert.That(c2, Is.EqualTo(2));
        }

        [Test]
        public void SetCardsAsMatched_InvokesEvent()
        {
            bool eventFired = false;
            GameServiceManager.Instance.CardsMatched += (idx1, idx2) => eventFired = true;

            GameServiceManager.Instance.SetCardsAsMatched(3, 4);

            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void UpdateScore_InvokesEventWithCorrectScore()
        {
            int score = -1;
            GameServiceManager.Instance.ScoreUpdated += (player, s) => score = s;

            GameServiceManager.Instance.UpdateScore("Player1", 100);

            Assert.That(score, Is.EqualTo(100));
        }


        [Test]
        public void GameFinished_InvokesEventWithCorrectWinner()
        {
            string winner = null;
            GameServiceManager.Instance.GameFinished += (w) => winner = w;

            ((IGameLobbyServiceCallback)GameServiceManager.Instance).GameFinished("WinnerUser");

            Assert.That(winner, Is.EqualTo("WinnerUser"));
        }

        #endregion

        #region Service Wrapper Tests (Error Handling)

        [Test]
        public async Task CreateLobbyAsync_ReturnsFalse_WhenServerOffline()
        {
            bool result = await GameServiceManager.Instance.CreateLobbyAsync("token", "matchCode", false);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task JoinLobbyAsync_ReturnsFalse_WhenServerOffline()
        {
            bool result = await GameServiceManager.Instance.JoinLobbyAsync("token", "code", false, "user");
            Assert.That(result, Is.False);
        }

        [Test]
        public void StartGameSafe_DoesNotCrash_WhenServerOffline()
        {
            Assert.DoesNotThrow(() =>
                GameServiceManager.Instance.StartGameSafe(new GameSettings()));
        }

        [Test]
        public void LeaveLobbyAsync_DoesNotCrash_WhenServerOffline()
        {
            Assert.DoesNotThrowAsync(async () =>
                await GameServiceManager.Instance.LeaveLobbyAsync());
        }

        [Test]
        public async Task SendInvitationEmailAsync_ReturnsFalse_WhenServerOffline()
        {
            bool result = await GameServiceManager.Instance.SendInvitationEmailAsync("test@email.com", "Subj", "Body");
            Assert.That(result, Is.False);
        }

        [Test]
        public void FlipCardAsync_DoesNotCrash_WhenServerOffline()
        {
            Assert.DoesNotThrowAsync(async () =>
                await GameServiceManager.Instance.FlipCardAsync(1));
        }

        [Test]
        public void SendChatMessageAsync_DoesNotCrash_WhenServerOffline()
        {
            Assert.DoesNotThrowAsync(async () =>
                await GameServiceManager.Instance.SendChatMessageAsync("hello"));
        }

        [Test]
        public void VoteToKickAsync_DoesNotCrash_WhenServerOffline()
        {
            Assert.DoesNotThrowAsync(async () =>
                await GameServiceManager.Instance.VoteToKickAsync("PlayerBad"));
        }

        #endregion
    }
}
