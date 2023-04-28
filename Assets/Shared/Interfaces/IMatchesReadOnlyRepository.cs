using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;

namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesReadOnlyRepository
    {
        Result<List<Match>> GetAll();
        Result<Match> Get(int id);
    }
}
