using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface IStartTurnPresenter
    {
        void UpdateView(TurnDto turnDto);
        void OnLoadEventHandler(int userId, int roundId);
    }
}