using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface ICreateBotMatchUseCase
    {
        Result<MatchDto> Create(int userId);
    }
}
