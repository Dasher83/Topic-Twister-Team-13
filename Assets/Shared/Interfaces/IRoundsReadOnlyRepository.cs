using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IRoundsReadOnlyRepository
    {
        Result<List<Round>> GetAll();
        Result<Round> Get(int id);
        Result<List<Round>> GetMany(List<int> ids);

    }
}
