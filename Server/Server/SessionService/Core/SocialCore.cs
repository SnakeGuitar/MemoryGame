using Server.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Server.SessionService.Core
{
    internal class SocialCore
    {
        private readonly IDbContextFactory _dbFactory;
        private readonly ISessionManager _sessionManager;
        private readonly ILoggerManager _logger;

        public SocialCore(IDbContextFactory dbFactory, ISessionManager sessionManager, ILoggerManager logger)
        {
            _dbFactory = dbFactory;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        public ResponseDTO SendFriendRequest(string token, string targetUsername)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);
            if (userId == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };

            try
            {
                using (var db = _dbFactory.Create())
                {
                    var sender = db.user.Include("user1").Include("user2").FirstOrDefault(u => u.userId == userId.Value);
                    var target = db.user.FirstOrDefault(u => u.username == targetUsername);

                    if (target == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };
                    if (sender.userId == target.userId) return new ResponseDTO { Success = false, MessageKey = "Social_Error_SelfAdd" };

                    bool alreadyFriends = sender.user1.Any(u => u.userId == target.userId) ||
                                          sender.user2.Any(u => u.userId == target.userId);

                    if (alreadyFriends) return new ResponseDTO { Success = false, MessageKey = "Social_Error_AlreadyFriends" };

                    bool existingRequest = db.FriendRequest.Any(r =>
                        (r.senderId == sender.userId && r.receiverId == target.userId) ||
                        (r.senderId == target.userId && r.receiverId == sender.userId));

                    if (existingRequest) return new ResponseDTO { Success = false, MessageKey = "Social_Error_RequestExists" };

                    var request = new FriendRequest
                    {
                        senderId = sender.userId,
                        receiverId = target.userId,
                        sentAt = DateTime.UtcNow
                    };

                    db.FriendRequest.Add(request);
                    db.SaveChanges();

                    return new ResponseDTO { Success = true };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"Database error in SendFriendRequest: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_DatabaseError" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in SendFriendRequest: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_UnexpectedError" };
            }
        }

        public List<FriendRequestDTO> GetPendingRequests(string token)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);
            if (userId == null) return null;


            try
            {
                using (var db = _dbFactory.Create())
                {
                    var requests = db.FriendRequest
                        .Include("user")
                        .Where(r => r.receiverId == userId.Value)
                        .ToList();

                    return requests.Select(r => new FriendRequestDTO
                    {
                        RequestId = r.requestId,
                        SenderUsername = r.user.username,
                        SenderAvatar = r.user.avatar
                    }).ToList();
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"Database error in GetPendingRequests: {ex.Message}");
                return new List<FriendRequestDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetPendingRequests: {ex.Message}");
                return new List<FriendRequestDTO>();
            }
        }

        public ResponseDTO AnswerFriendRequest(string token, int requestId, bool accept)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);
            if (userId == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };

            try
            {
                using (var db = _dbFactory.Create())
                {
                    var request = db.FriendRequest.FirstOrDefault(r => r.requestId == requestId);

                    if (request == null) return new ResponseDTO { Success = false, MessageKey = "Social_Error_RequestNotFound" };
                    if (request.receiverId != userId.Value) return new ResponseDTO { Success = false, MessageKey = "Global_Error_Unauthorized" };

                    if (accept)
                    {
                        var sender = db.user.FirstOrDefault(u => u.userId == request.senderId);
                        var receiver = db.user.Include("user1").FirstOrDefault(u => u.userId == request.receiverId);

                        if (sender != null && receiver != null)
                        {
                            receiver.user1.Add(sender);
                        }
                    }

                    db.FriendRequest.Remove(request);
                    db.SaveChanges();

                    return new ResponseDTO { Success = true };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"Database error in AnswerFriendRequest: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_DatabaseError" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in AnswerFriendRequest: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_UnexpectedError" };
            }
        }

        public List<FriendDTO> GetFriendsList(string token)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);
            if (userId == null) return null;

            try
            {
                using (var db = _dbFactory.Create())
                {
                    var user = db.user.Include("user1").Include("user2").FirstOrDefault(u => u.userId == userId.Value);
                    if (user == null) return new List<FriendDTO>();

                    var allFriends = user.user1.Union(user.user2).ToList();

                    return allFriends.Select(f => new FriendDTO
                    {
                        Username = f.username,
                        Avatar = f.avatar,
                        IsOnline = _sessionManager.IsUserOnline(f.email)
                    }).ToList();
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"Database error in GetFriendsList: {ex.Message}");
                return new List<FriendDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetFriendsList: {ex.Message}");
                return new List<FriendDTO>();
            }
        }

        public ResponseDTO RemoveFriend(string token, string friendUsername)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);
            if (userId == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };

            try
            {
                using (var db = _dbFactory.Create())
                {
                    var currentUser = db.user.Include("user1").Include("user2").FirstOrDefault(u => u.userId == userId.Value);
                    var friendUser = db.user.FirstOrDefault(u => u.username == friendUsername);

                    if (friendUser == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };

                    bool removed = false;

                    var friendIn1 = currentUser.user1.FirstOrDefault(u => u.userId == friendUser.userId);
                    if (friendIn1 != null)
                    {
                        currentUser.user1.Remove(friendIn1);
                        removed = true;
                    }

                    var friendIn2 = currentUser.user2.FirstOrDefault(u => u.userId == friendUser.userId);
                    if (friendIn2 != null)
                    {
                        currentUser.user2.Remove(friendIn2);
                        removed = true;
                    }

                    if (removed)
                    {
                        db.SaveChanges();
                        return new ResponseDTO { Success = true };
                    }

                    return new ResponseDTO { Success = false, MessageKey = "Social_Error_NotFriends" };
                }
            }
            catch (EntityException ex)
            {
                _logger.LogError($"Database error in RemoveFriend: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_DatabaseError" };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in RemoveFriend: {ex.Message}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_UnexpectedError" };
            }
        }
    }
}
