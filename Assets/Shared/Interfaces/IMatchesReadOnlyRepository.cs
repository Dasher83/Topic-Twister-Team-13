using System.Collections.Generic;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesReadOnlyRepository
    {
        List<Match> GetAll();
        Match Get(int id);
    }
}
