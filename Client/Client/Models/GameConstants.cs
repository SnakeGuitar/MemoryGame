namespace Client.Models
{
    public static class GameConstants
    {
        #region Match configuration
        public const int DefaultCardCount = 16;
        public const int DefaultTurnTimeSeconds = 30;

        public const int MinCardCount = 4;
        public const int MaxCardCount = 40;

        public const int MinTurnTimeSeconds = 5;
        public const int MinTurnTimeSecondsFallback = 10;
        #endregion

        #region Resources Paths
        public const string ColorCardFrontBasePath = "/Client;component/Resources/Images/Cards/Fronts/Color/";
        public const string NormalCardFrontBasePath = "/Client;component/Resources/Images/Cards/Fronts/Normal/";
        public const string CardBackPath = "/Client;component/Resources/Images/Cards/Backs/Back.png";
        #endregion

        #region Animation Delays (in milliseconds)
        public const int MatchFeedbackDelay = 500;
        public const int MismatchFeedbackDelay = 1000;
        #endregion

        #region Scores
        public const int PointsPerMatch = 10;
        #endregion

        #region Lobby limits
        public const int MinPlayersToPlay = 2;
        public const int MaxPlayersPerLobby = 4;
        #endregion
    }
}
