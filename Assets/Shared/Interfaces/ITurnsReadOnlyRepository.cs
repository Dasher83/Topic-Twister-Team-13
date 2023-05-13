using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface ITurnsReadOnlyRepository
    {
        Operation<List<Turn>> GetAll();
        Operation<Turn> Get(int userId, int roundId);
        Operation<List<Turn>> GetMany(int userId, Match match);
    }
}
