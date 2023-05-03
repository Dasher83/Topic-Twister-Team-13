using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IRoundsReadOnlyRepository
    {
        Operation<List<Round>> GetAll();
        Operation<Round> Get(int id);
        Operation<List<Round>> GetMany(List<int> ids);
        Operation<List<Round>> GetMany(int matchId);
    }
}
