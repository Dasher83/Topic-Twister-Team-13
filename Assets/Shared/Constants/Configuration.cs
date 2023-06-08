
namespace TopicTwister.Shared.Constants
{
    public static class Configuration
    {
        public const float TurnDurationInSeconds = 60f;

        public const float TurnToleranceInSeconds = 10f;

        public const float TurnDurationInSecondsPlusTolerance = TurnDurationInSeconds + TurnToleranceInSeconds;

        public const int CategoriesPerRound = 5;

        public const int RoundsPerMatch = 3;

        public const int PlayersPerMatch = 2;

        public const int TestUserId = 1;

        public const int TestBotId = 2;

        public const int ExtraUserOneId = 3;
        public const int ExtraUserTwoId = 4;

        public static class Scenes
        {
            public const string PlayTurn = "PlayTurn";
            public const string TurnResultScene = "TurnResultScene";
            public const string BeginRoundScene = "BeginRoundScene";
            public const string RoundResults = "RoundResults";
            public const string Home = "Home";
            public const string MatchResult = "MatchResult";
        }

        public static class TransitionsDuration
        {
            public const float FromBeginRoundToPlayTurn = 2f;
            public const float FromPlayTurnToTurnResult = 1f;
        }
    }
}
