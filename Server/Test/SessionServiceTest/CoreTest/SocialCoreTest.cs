using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Server;
using Server.SessionService;
using Server.SessionService.Core;
using Server.Shared;
using System;
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
        public void SendFriendRequest_InvalidToken_ReturnsFalse()
        {
            string token = "invalid_token";
            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns((int?)null);

            var result = _socialCore.SendFriendRequest(token, "TargetUser");

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public void SendFriendRequest_TargetNotFound_ReturnsUserNotFoundError()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var me = new user { userId = myId };
            var users = new List<user> { me }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _socialCore.SendFriendRequest(token, "GhostUser");

            Assert.AreEqual("Global_Error_UserNotFound", result.MessageKey);
        }

        [TestMethod]
        public void SendFriendRequest_SelfRequest_ReturnsSelfAddError()
        {
            string token = "valid_token";
            int myId = 1;
            string myName = "Me";

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var me = new user { userId = myId, username = myName };
            var users = new List<user> { me }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _socialCore.SendFriendRequest(token, myName);

            Assert.AreEqual("Social_Error_SelfAdd", result.MessageKey);
        }

        [TestMethod]
        public void SendFriendRequest_AlreadyFriends_ReturnsAlreadyFriendsError()
        {
            string token = "valid_token";
            int myId = 1;
            int friendId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var friend = new user { userId = friendId, username = "Friend" };

            var me = new user { userId = myId, user1 = new List<user> { friend }, user2 = new List<user>() };

            var users = new List<user> { me, friend }.AsQueryable();
            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(users).Object);

            var result = _socialCore.SendFriendRequest(token, "Friend");

            Assert.AreEqual("Social_Error_AlreadyFriends", result.MessageKey);
        }

        [TestMethod]
        public void SendFriendRequest_RequestExists_ReturnsRequestExistsError()
        {
            string token = "valid_token";
            int myId = 1;
            int targetId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var me = new user { userId = myId, user1 = new List<user>(), user2 = new List<user>() };
            var target = new user { userId = targetId, username = "Target" };

            var existingReq = new FriendRequest { senderId = myId, receiverId = targetId };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, target }.AsQueryable()).Object);
            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest> { existingReq }.AsQueryable()).Object);

            var result = _socialCore.SendFriendRequest(token, "Target");

            Assert.AreEqual("Social_Error_RequestExists", result.MessageKey);
        }

        [TestMethod]
        public void SendFriendRequest_ValidRequest_AddsRequestToDb()
        {
            string token = "valid_token";
            int myId = 1;
            int targetId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var me = new user { userId = myId, user1 = new List<user>(), user2 = new List<user>() };
            var target = new user { userId = targetId, username = "Target" };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, target }.AsQueryable()).Object);

            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest>().AsQueryable()).Object);

            _socialCore.SendFriendRequest(token, "Target");

            _mockContext.Verify(c => c.FriendRequest.Add(It.Is<FriendRequest>(r => r.senderId == myId && r.receiverId == targetId)), Times.Once);
        }

        // === GetPendingRequests Tests ===

        [TestMethod]
        public void GetPendingRequests_HasRequests_ReturnsList()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var sender = new user { username = "SenderUser", avatar = new byte[0] };
            var req = new FriendRequest { requestId = 10, receiverId = myId, senderId = 2, user = sender };

            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest> { req }.AsQueryable()).Object);

            var result = _socialCore.GetPendingRequests(token);

            Assert.AreEqual(1, result.Count);
        }

        // === AnswerFriendRequest Tests ===

        [TestMethod]
        public void AnswerFriendRequest_RequestNotFound_ReturnsError()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);
            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest>().AsQueryable()).Object);

            var result = _socialCore.AnswerFriendRequest(token, 999, true);

            Assert.AreEqual("Social_Error_RequestNotFound", result.MessageKey);
        }

        [TestMethod]
        public void AnswerFriendRequest_NotReceiver_ReturnsUnauthorized()
        {
            string token = "valid_token";
            int myId = 1;
            int otherPersonId = 2;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var req = new FriendRequest { requestId = 10, receiverId = otherPersonId };
            _mockContext.Setup(c => c.FriendRequest).Returns(DbContextMockFactory.CreateMockDbSet(new List<FriendRequest> { req }.AsQueryable()).Object);

            var result = _socialCore.AnswerFriendRequest(token, 10, true);

            Assert.AreEqual("Global_Error_Unauthorized", result.MessageKey);
        }

        [TestMethod]
        public void AnswerFriendRequest_Accept_AddsFriendship()
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

            _socialCore.AnswerFriendRequest(token, 10, true);

            Assert.IsTrue(me.user1.Contains(sender));
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

            var f1 = new user { username = "F1", email = "f1@test.com" };
            var f2 = new user { username = "F2", email = "f2@test.com" };

            var me = new user { userId = myId, user1 = new List<user> { f1 }, user2 = new List<user> { f2 } };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me }.AsQueryable()).Object);
            _mockSession.Setup(s => s.IsUserOnline(It.IsAny<string>())).Returns(true);

            var result = _socialCore.GetFriendsList(token);

            Assert.AreEqual(2, result.Count);
        }

        // === RemoveFriend Tests ===

        [TestMethod]
        public void RemoveFriend_NotFriends_ReturnsError()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var me = new user { userId = myId, user1 = new List<user>(), user2 = new List<user>() };
            var stranger = new user { userId = 2, username = "Stranger" };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, stranger }.AsQueryable()).Object);

            var result = _socialCore.RemoveFriend(token, "Stranger");

            Assert.AreEqual("Social_Error_NotFriends", result.MessageKey);
        }

        [TestMethod]
        public void RemoveFriend_IsFriend_ReturnsSuccess()
        {
            string token = "valid_token";
            int myId = 1;

            _mockSession.Setup(s => s.GetUserIdFromToken(token)).Returns(myId);

            var exFriend = new user { userId = 2, username = "ExFriend" };
            var me = new user { userId = myId, user1 = new List<user> { exFriend }, user2 = new List<user>() };

            _mockContext.Setup(c => c.user).Returns(DbContextMockFactory.CreateMockDbSet(new List<user> { me, exFriend }.AsQueryable()).Object);

            var result = _socialCore.RemoveFriend(token, "ExFriend");

            Assert.IsTrue(result.Success);
        }
    }
}