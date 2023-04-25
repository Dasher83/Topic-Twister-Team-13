using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICreateRoundGatewayService
    {
        RoundWithCategoriesDto Create(MatchDTO matchDto);
    }
}
