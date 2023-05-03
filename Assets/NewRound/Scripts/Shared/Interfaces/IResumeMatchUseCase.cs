using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IResumeMatchUseCase
    {
        Operation<RoundWithCategoriesDto> Execute(MatchDto matchDto);
    }
}
