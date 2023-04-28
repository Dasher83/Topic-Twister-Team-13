using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface ICreateBotMatchUseCase
    {
        Result<MatchDTO> Create(int userId);
    }
}
