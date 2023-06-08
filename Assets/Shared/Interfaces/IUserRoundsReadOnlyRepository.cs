using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserRoundsReadOnlyRepository
    {
        Operation<List<UserRound>> GetAll();
        Operation<UserRound> Get(int userId, int roundId);
        Operation<List<UserRound>> GetMany(int userId, List<int> roundIds);
    }
}
