using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface ICreateMatchSubUseCase
    {
        Operation<MatchDto> Create(int userIdPlayerOne, int userIdPlayerTwo);
    }
}