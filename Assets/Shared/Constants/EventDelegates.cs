using TopicTwister.Shared.DTOs;


namespace TopicTwister.Shared.Constants
{
    public static class EventDelegates
    {
        public static class IPlayTurnView
        {
            public delegate void LoadEventHandler(int userId, int matchId);
        }

        public static class ITurnResultView
        {
            public delegate void LoadEventHandler();
        }

        public static class IResumeMatchView
        {
            public delegate void LoadEventHandler(MatchDto matchDto);
        }

        public static class IStartBotMatchView
        {
            public delegate void StartMatchVersusBotEventHandler();
        }
    }
}
