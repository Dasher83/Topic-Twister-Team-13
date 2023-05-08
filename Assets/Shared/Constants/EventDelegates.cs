namespace TopicTwister.Shared.Constants
{
    public static class EventDelegates
    {
        public static class PlayTurnView
        {
            public delegate void LoadEventHandler(int userId, int matchId);
        }
    }
}