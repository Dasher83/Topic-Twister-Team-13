using TopicTwister.Shared.DTOs;


namespace TopicTwister.Home.Shared.Interfaces
{
    public interface ICreateBotMatchUseCase
    {
        MatchDTO Create(int userId);
    }
}
