using System.Collections.Generic;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository: IMatchesReadOnlyRepository
    {
        Match Persist(Match match);
        void Delete(int id);
    }
}
