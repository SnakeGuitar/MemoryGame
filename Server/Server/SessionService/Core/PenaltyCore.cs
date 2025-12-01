using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.SessionService.Core
{
    internal class PenaltyCore
    {
        private readonly IDbContextFactory _dbFactory;
        private readonly ISessionManager _sessionManager;
        private readonly ILoggerManager _logger;

        public PenaltyCore(IDbContextFactory dbFactory, ISessionManager sessionManager, ILoggerManager logger)
        {
            _dbFactory = dbFactory;
            _sessionManager = sessionManager;
            _logger = logger;
        }

        public ResponseDTO ReportUser(string token, string targetUsername, int matchId)
        {
            var reporterId = _sessionManager.GetUserIdFromToken(token);
            if (reporterId == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_InvalidToken" };

            using (var db = _dbFactory.Create())
            {
                var target = db.user.FirstOrDefault(u => u.username == targetUsername);
                if (target == null) return new ResponseDTO { Success = false, MessageKey = "Global_Error_UserNotFound" };

                var penalty = new penalty
                {
                    type = 1,
                    duration = DateTime.UtcNow.AddHours(24),
                    matchId = matchId
                };

                db.penalty.Add(penalty);
                db.SaveChanges();

                target.penaltyId = penalty.penaltyId;
                db.SaveChanges();

                _logger.LogInfo($"User {targetUsername} was penalized by {reporterId} in match {matchId}");
                return new ResponseDTO { Success = true };
            }
        }
    }
}
