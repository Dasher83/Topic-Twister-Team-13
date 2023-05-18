using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchesRepository
    {
        Operation<UserMatch> Insert(UserMatch userMatch);
        Operation<UserMatch> Update(UserMatch userMatch);
        Operation<UserMatch> Get(int userId, int matchId);
        Operation<UserMatch[]> GetMany(int matchId);
        Operation<List<UserMatch>> GetAll();
    }
}
