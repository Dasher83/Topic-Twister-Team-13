using System;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IResumeMatchView
    {
        event EventDelegates.IResumeMatchView.LoadEventHandler Load;
        void UpdateMatchData(RoundWithCategoriesDto roundWithCategoriesDto);
    }
}
