using Server.Shared;
using Server.Validator;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SessionService.Core
{
    internal class UserProfileCore
    {
        private readonly IDbContextFactory _dbFactory;
        private readonly ISessionManager _sessionManager;
        private readonly ISecurityService _securityService;
        private readonly ILoggerManager _logger;

        public UserProfileCore(
            DbContextFactory dbFactory,
            ILoggerManager logger,
            ISessionManager sessionManager,
            ISecurityService securityService)
        {
            _dbFactory = dbFactory;
            _logger = logger;
            _sessionManager = sessionManager;
            _securityService = securityService;
        }

        public byte[] GetUserAvatar(string email)
        {
            try
            {
                using (var db = _dbFactory.Create())
                {
                    var user = db.user.FirstOrDefault(u => u.email == email);

                    _logger.LogInfo($"GetUserAvatar called for email: {email}");
                    return user?.avatar;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetUserAvatar Error for email {email}: {ex.Message}");
                return null;
            }
        }

        public ResponseDTO UpdateUserAvatar(string token, byte[] avatar)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);

            if (userId == null)
            {
                _logger.LogWarn($"UpdateUserAvatar called with invalid token.");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };
            }

            if (avatar != null && !ImageValidator.IsValidImage(avatar))
            {
                _logger.LogWarn($"UpdateUserAvatar called with invalid image data for userId: {userId.Value}");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_ImageInvalid" };
            }

            using (var db = _dbFactory.Create())
            {
                var user = db.user.FirstOrDefault(u => u.userId == userId.Value);

                if (user == null)
                {
                    _logger.LogInfo($"UpdateUserAvatar could not find user with userId: {userId.Value}");
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };
                }

                user.avatar = avatar;
                db.SaveChanges();

                _logger.LogInfo($"User avatar updated successfully for userId: {userId.Value}");
                return new ResponseDTO { Success = true };
            }
        }

        public ResponseDTO ChangePassword(string token, string currentPassword, string newPassword)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);

            if (userId == null)
            {
                _logger.LogWarn($"ChangePassword called with invalid token");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };
            }

            if (string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword))
            {
                _logger.LogWarn("Password arguments are null or empty");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_PasswordNull" };
            }

            using (var db = _dbFactory.Create())
            {
                var user = db.user.FirstOrDefault(u => u.userId == userId.Value);

                if (user == null)
                {
                    _logger.LogInfo($"ChangePassword could not find user with userId: {userId.Value}");
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };
                }

                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.password))
                {
                    _logger.LogInfo("Entered password doesn't match actual password");
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_PasswordUpdate" };
                }

                user.password = _securityService.HashPassword(newPassword);
                db.SaveChanges();

                _logger.LogInfo($"Password updated successfully for userId: {userId.Value}");
                return new ResponseDTO { Success = true };
            }
        }

        public ResponseDTO ChangeUsername(string token, string newUsername)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);

            if (userId == null)
            {
                _logger.LogInfo($"ChangeUsername called with invalid token");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };
            }

            if (string.IsNullOrEmpty(newUsername))
            {
                _logger.LogWarn($"newUsername is null");
                return new ResponseDTO { Success = false, MessageKey = "Global_Error_NewUsernameIsNull" };
            }

            using (var db = _dbFactory.Create())
            {
                var user = db.user.FirstOrDefault(u => u.userId == userId.Value);

                if (user == null)
                {
                    _logger.LogInfo($"ChangeUsername could not find user with userId: {userId.Value}");
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };
                }

                if (user.username == newUsername)
                {
                    _logger.LogInfo($"New username has the same value as current username");
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_SameUsername" };
                }

                if (db.user.Any(u => u.username == newUsername))
                {
                    _logger.LogInfo($"Username {newUsername} is already taken.");
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_UsernameInUse" };
                }

                user.username = newUsername;
                db.SaveChanges();

                _logger.LogInfo($"Username updated successfully for userId: {userId.Value}");
                return new ResponseDTO { Success = true };
            }
        }
    }
}
