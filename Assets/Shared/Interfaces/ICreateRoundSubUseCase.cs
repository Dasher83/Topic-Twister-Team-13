using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface ICreateRoundSubUseCase
    {
        Operation<RoundWithCategoriesDto> Execute(MatchDto matchDto);
    }
}
