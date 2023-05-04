using System;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IResumeMatchView
    {
        event Action<MatchDto> OnLoad;
        void UpdateMatchData(RoundWithCategoriesDto roundWithCategoriesDto);
    }
}
