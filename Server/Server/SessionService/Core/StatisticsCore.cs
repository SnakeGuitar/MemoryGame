using Server.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Server.SessionService.Core
{
    public class StatisticsCore
    {
        private readonly IDbContextFactory _dbFactory;
        private readonly ISessionManager _sessionManager;
        private readonly ILoggerManager _loggerManager;

        public StatisticsCore(IDbContextFactory dbFactory, ISessionManager sessionManager, ILoggerManager loggerManager)
        {
            _dbFactory = dbFactory;
            _sessionManager = sessionManager;
            _loggerManager = loggerManager;
        }

        public List<MatchHistoryDTO> GetMatchHistory(string token)
        {
            var userId = _sessionManager.GetUserIdFromToken(token);
            if (userId == null) return null;

            try
            {
                using (var db = _dbFactory.Create())
                {
                    var history = db.matchHistory
                        .Include("match")
                        .Include("user")
                        .Where(m => m.userId == userId.Value)
                        .OrderByDescending(m => m.match.endDateTime)
                        .ToList();

                    return history.Select(h => new MatchHistoryDTO
                    {
                        MatchId = h.matchId,
                        Date = h.match.endDateTime,
                        Score = h.score,

                        WinnerName = h.winnerId.HasValue
                        ? (db.user.Find(h.winnerId)?.username ?? "Unknown")
                        : "Guest"
                    }).ToList();
                }
            }
            catch (EntityException ex)
            {
                _loggerManager.LogError($"GetMatchHistory Database Error for userId {userId.Value}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetMatchHistory Error for userId {userId.Value}: {ex.Message}");
                return null;
            }
        }
    }
}
