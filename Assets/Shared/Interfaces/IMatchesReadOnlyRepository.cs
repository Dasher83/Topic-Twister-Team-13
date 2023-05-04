using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesReadOnlyRepository
    {
        Operation<List<Match>> GetAll();
        Operation<Match> Get(int id);
    }
}
