using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundUseCase
    {
        RoundWithCategoriesDto Create(MatchDTO matchDto);
    }
}
