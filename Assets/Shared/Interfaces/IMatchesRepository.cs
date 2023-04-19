using System.Collections.Generic;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository
    {
        Match Persist(Match match);
        List<Match> GetAll();
        Match Get(int id);
    }
}
