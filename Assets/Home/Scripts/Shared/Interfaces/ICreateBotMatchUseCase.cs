using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.UseCases.Utils;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface ICreateBotMatchUseCase
    {
        UseCaseResult<MatchDTO> Create(int userId);
    }
}
