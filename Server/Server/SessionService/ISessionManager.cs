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
    }

    public class SessionManager : ISessionManager
    {
        private const int SESSION_DURATION_HOURS = 2;

        private readonly IDbContextFactory _dbFactory;
        public SessionManager(IDbContextFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public SessionManager() : this(new DbContextFactory())
        {
        }

        public int? GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            using (var db = _dbFactory.Create())
            {
                var session = db.userSession
                    .FirstOrDefault(s => s.token == token && s.expiresAt > DateTime.Now);

                return session?.userId;
            }
        }

        public string CreateSessionToken(int userId)
        {
            using (var db = _dbFactory.Create())
            {
                var expiredSessions = db.userSession.Where(s => s.expiresAt <= DateTime.Now);
                db.userSession.RemoveRange(expiredSessions);

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

                return token;
            }
        }
    }
}
