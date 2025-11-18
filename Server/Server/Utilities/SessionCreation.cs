using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Utilities
{
    internal class SessionCreation
    {
        private const int SESSION_DURATION_HOURS = 2;   
        internal int? GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            try
            {
                using (var db = new memoryGameDBEntities())
                {
                    var session = db.userSession
                        .FirstOrDefault(s => s.token == token && s.expiresAt > DateTime.Now);
                    return session?.userId;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserIdFromToken Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public string CreateSessionToken(int userId)
        {
            using (var db = new memoryGameDBEntities())
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
