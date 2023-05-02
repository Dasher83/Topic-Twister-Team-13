using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundUseCase
    {
        Result<RoundWithCategoriesDto> Create(MatchDto matchDto);
    }
}
