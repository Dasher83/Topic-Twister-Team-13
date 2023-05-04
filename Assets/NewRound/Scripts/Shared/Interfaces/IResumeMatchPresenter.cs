using System;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IResumeMatchPresenter
    {
        void ResumeMatch(MatchDto matchDto);
        void UpdateView(RoundWithCategoriesDto roundWithCategoriesDto);
    }
}
