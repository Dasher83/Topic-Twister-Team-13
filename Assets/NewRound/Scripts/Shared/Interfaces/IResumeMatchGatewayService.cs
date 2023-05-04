using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IResumeMatchGatewayService
    {
        RoundWithCategoriesDto Create(MatchDto matchDto);
    }
}
