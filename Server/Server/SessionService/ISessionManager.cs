using Server.Shared;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;

namespace Server.SessionService
{
    public interface ISessionManager
    {
        string CreateSessionToken(int userId);
        bool RenewSession(string token);
        int? GetUserIdFromToken(string token);
        bool IsUserOnline(string email);
        void RegisterUserCallback(int userId);
    }

    public class SessionManager : ISessionManager
    {
        private const int SESSION_DURATION_MINUTES = 10;
        private static ConcurrentDictionary<int, IUserCallback> _activeUserCallbacks = new ConcurrentDictionary<int, IUserCallback>();

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
                    .FirstOrDefault(s => s.token == token);

                if (session == null)
                {
                    _logger.LogInfo($"No session found for token: {token}");
                    return null;
                }

                if (session.expiresAt < DateTime.UtcNow)
                {
                    _logger.LogInfo($"Token found but expired. ExpiresAt: {session.expiresAt}, UtcNow: {DateTime.UtcNow}");

                    db.userSession.Remove(session);
                    db.SaveChanges();

                    return null;
                }


                _logger.LogInfo($"Valid session found for token: {token}");
                return session.userId;
            }
        }

        public string CreateSessionToken(int userId)
        {
            RegisterUserCallback(userId);

            using (var db = _dbFactory.Create())
            {
                var userSessions = db.userSession.Where(s => s.userId == userId);
                db.userSession.RemoveRange(userSessions);

                string token = Guid.NewGuid().ToString("N");

                var session = new userSession
                {
                    token = token,
                    userId = userId,
                    createdAt = DateTime.UtcNow,
                    expiresAt = DateTime.UtcNow.AddMinutes(SESSION_DURATION_MINUTES)
                };

                db.userSession.Add(session);
                db.SaveChanges();

                _logger.LogInfo($"Created new session for userId {userId} with token: {token}");
                return token;
            }
        }

        public bool RenewSession(string token)
        {
            using (var db = _dbFactory.Create())
            {
                var session = db.userSession.FirstOrDefault(s => s.token == token);
                if (session != null)
                {
                    session.expiresAt = DateTime.UtcNow.AddMinutes(SESSION_DURATION_MINUTES);
                    db.SaveChanges();
                    _logger.LogInfo($"Renew session token: {token}");
                    return true;
                }

                _logger.LogInfo($"Couldn't renew session for userId {session.userId}");
                return false;
            }
        }

        public bool IsUserOnline(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            using (var db = _dbFactory.Create())
            {
                return db.userSession.Any(s => s.user.email == email && s.expiresAt > DateTime.UtcNow);
            }
        }

        public void RegisterUserCallback(int userId)
        {
            var callback = OperationContext.Current.GetCallbackChannel<IUserCallback>();
            if (callback != null)
            {
                if (_activeUserCallbacks.TryGetValue(userId, out var oldCallback))
                {
                    try
                    {
                        if (((ICommunicationObject)oldCallback).State == CommunicationState.Opened)
                        {
                            oldCallback.ForceLogout("Logged in from another location");
                        }
                    }
                    catch 
                    {
                        
                    }
                }
                _activeUserCallbacks.AddOrUpdate(userId, callback, (k, v) => callback);
            }
        }

    }
}