using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchesRepository
    {
        Result<UserMatch> Save(UserMatch userMatch);
        Result<UserMatch> Get(int userId, int matchId);
        Result<List<UserMatch>> GetAll();
    }
}
