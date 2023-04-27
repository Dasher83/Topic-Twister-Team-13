using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.UseCases.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundUseCase
    {
        UseCaseResult<RoundWithCategoriesDto> Create(MatchDTO matchDto);
    }
}
