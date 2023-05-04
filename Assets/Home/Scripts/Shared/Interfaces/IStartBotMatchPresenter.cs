using TopicTwister.Shared.DTOs;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface IStartBotMatchPresenter
    {
        void StartMatchVersusBot();
        void UpdateView(MatchDto matchDto);
    }
}
