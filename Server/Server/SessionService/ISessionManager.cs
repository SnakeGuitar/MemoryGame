using log4net.Core;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SessionService
{
    public interface ISessionManager
    {
        string CreateSessionToken(int userId);
        int? GetUserIdFromToken(string token);
        bool IsUserOnline(string email);
    }

    public class SessionManager : ISessionManager
    {
        private const int SESSION_DURATION_HOURS = 2;

        private readonly IDbContextFactory _dbFactory;
        private readonly ILoggerManager _logger;
        public SessionManager(IDbContextFactory dbFactory, ILoggerManager logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public SessionManager() : this(
            new DbContextFactory(),
            new Logger(typeof(SessionManager)))
        {
        }

        public int? GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarn("GetUserIdFromToken called with null or empty token.");
                return null;
            }

            using (var db = _dbFactory.Create())
            {
                var session = db.userSession
                    .FirstOrDefault(s => s.token == token && s.expiresAt > DateTime.Now);

                _logger.LogInfo(session != null
                    ? $"Valid session found for token: {token}"
                    : $"No valid session found for token: {token}");
                return session?.userId;
            }
        }

        public string CreateSessionToken(int userId)
        {
            using (var db = _dbFactory.Create())
            {
                var userSessions = db.userSession.Where(s => s.userId == userId);
                db.userSession.RemoveRange(userSessions);

                string token = Guid.NewGuid().ToString("N");

                var session = new userSession
                {
                    token = token,
                    userId = userId,
                    createdAt = DateTime.Now,
                    expiresAt = DateTime.Now.AddHours(SESSION_DURATION_HOURS)
                };

                db.userSession.Add(session);
                db.SaveChanges();

                _logger.LogInfo($"Created new session for userId {userId} with token: {token}");
                return token;
            }
        }

        public bool IsUserOnline(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            using (var db = _dbFactory.Create())
            {
                return db.userSession.Any(s => s.user.email == email && s.expiresAt > DateTime.Now);
            }
        }
    }
}
