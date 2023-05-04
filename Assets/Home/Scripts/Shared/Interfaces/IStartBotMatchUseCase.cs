using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface IStartBotMatchUseCase
    {
        Operation<MatchDto> Execute(int userId);
    }
}
