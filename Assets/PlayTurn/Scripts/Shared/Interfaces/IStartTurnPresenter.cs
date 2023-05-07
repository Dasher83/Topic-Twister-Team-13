namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IStartTurnPresenter
    {
        void OnLoadEventHandler(int userId, int roundId);
    }
}