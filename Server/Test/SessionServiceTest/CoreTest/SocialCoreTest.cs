using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.SessionService.Core;
using Server.Shared;
using System.Collections.Generic;
using System.Linq;
using Test.Helpers;

namespace Test.SessionServiceTest.CoreTest
{
    [TestClass]
    public class SocialCoreTest
    {
        private Mock<IDbContextFactory> _mockDbFactory;
        private Mock<memoryGameDBEntities> _mockContext;
        private Mock<ISessionManager> _mockSession;
        private Mock<ILoggerManager> _mockLogger;

        private SocialCore _socialCore;

        [TestInitialize]
        public void Setup()
        {
            _mockDbFactory = new Mock<IDbContextFactory>();
            _mockContext = new Mock<memoryGameDBEntities>();
            _mockSession = new Mock<ISessionManager>();
            _mockLogger = new Mock<ILoggerManager>();

            _mockDbFactory.Setup(f => f.Create()).Returns(_mockContext.Object);

            _socialCore = new SocialCore(
                _mockDbFactory.Object,
                _mockSession.Object,
                _mockLogger.Object
            );
        }

        // === SendFriendRequest Tests ===

        [TestMethod]
        public void SendFriendRequest_ValidRequest_AddsToDb()
        {
            string token = "valid_token";
            int myId = 1;
            int targetId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var me = new user { userId = myId, user1 = new List<user>(), user2 = new List<user>() };
            var target = new user { userId = targetId, username = "Target" };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, target }.AsQueryable()).Object);
            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest>().AsQueryable()).Object);

            var result = _socialCore.SendFriendRequest(token, "Target");

            Assert.IsTrue(result.Success, "SendFriendRequest failed: " + result.MessageKey);
            _mockContext.Verify(c => c.FriendRequest.Add(It.Is<FriendRequest>(r => r.senderId == myId && r.receiverId == targetId)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void SendFriendRequest_AlreadyFriends_ReturnsError()
        {
            string token = "valid_token";
            int myId = 1;
            int friendId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var friend = new user { userId = friendId, username = "Friend" };
            var me = new user { userId = myId, user1 = new List<user> { friend }, user2 = new List<user>() };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, friend }.AsQueryable()).Object);

            var result = _socialCore.SendFriendRequest(token, "Friend");

            Assert.AreEqual("Social_Error_AlreadyFriends", result.MessageKey);
        }

        // === GetPendingRequests Tests ===

        [TestMethod]
        public void GetPendingRequests_ReturnsList()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var sender = new user { username = "Sender", avatar = new byte[0] };
            var req = new FriendRequest { requestId = 10, receiverId = myId, user = sender };

            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest> { req }.AsQueryable()).Object);

            var result = _socialCore.GetPendingRequests(token);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Sender", result[0].SenderUsername);
        }

        // === AnswerFriendRequest Tests ===

        [TestMethod]
        public void AnswerFriendRequest_Accept_AddsFriendToCollection()
        {
            string token = "valid_token";
            int myId = 1;
            int senderId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var req = new FriendRequest { requestId = 10, receiverId = myId, senderId = senderId };

            var me = new user { userId = myId, user1 = new List<user>() };
            var sender = new user { userId = senderId };

            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest> { req }.AsQueryable()).Object);
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, sender }.AsQueryable()).Object);

            var result = _socialCore.AnswerFriendRequest(token, 10, true);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(me.user1.Contains(sender), "Sender should had added (user1) as a friend.");
            _mockContext.Verify(c => c.FriendRequest.Remove(req), Times.Once);
        }

        [TestMethod]
        public void AnswerFriendRequest_Decline_RemovesRequestOnly()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var req = new FriendRequest { requestId = 10, receiverId = myId, senderId = 2 };
            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest> { req }.AsQueryable()).Object);

            _socialCore.AnswerFriendRequest(token, 10, false);

            _mockContext.Verify(c => c.FriendRequest.Remove(req), Times.Once);
        }

        // === GetFriendsList Tests ===

        [TestMethod]
        public void GetFriendsList_ValidUser_ReturnsCombinedFriends()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);
            _mockSession.Setup(s => s.IsUserOnline(It.IsAny<string>())).Returns(true);

            var f1 = new user { userId = 10, username = "F1", email = "f1@test.com" };
            var f2 = new user { userId = 11, username = "F2", email = "f2@test.com" };

            var me = new user
            {
                userId = myId,
                user1 = new List<user> { f1 },
                user2 = new List<user> { f2 }
            };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, f1, f2 }.AsQueryable()).Object);

            var result = _socialCore.GetFriendsList(token);

            Assert.AreEqual(2, result.Count);
        }

        // === RemoveFriend Tests ===

        [TestMethod]
        public void RemoveFriend_FriendInList1_RemovesIt()
        {
            string token = "valid_token";
            int myId = 1;
            string friendName = "ExFriend";

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var friend = new user { userId = 2, username = friendName };
            var me = new user
            {
                userId = myId,
                user1 = new List<user> { friend },
                user2 = new List<user>()
            };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, friend }.AsQueryable()).Object);

            var result = _socialCore.RemoveFriend(token, friendName);

            Assert.IsTrue(result.Success, "RemoveFriend failed: " + result.MessageKey);
            Assert.IsFalse(me.user1.Contains(friend), "Friend should had been deleted from user1 list.");
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}