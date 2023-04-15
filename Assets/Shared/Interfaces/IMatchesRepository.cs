using TopicTwister.Shared.DTOs;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository
    {
        MatchDTO Create(int userOneId, int userTwoId);
    }
}
