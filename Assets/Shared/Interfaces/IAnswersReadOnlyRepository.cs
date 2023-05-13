using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IAnswersReadOnlyRepository
    {
        Operation<List<Answer>> GetMany(int roundId);
        Operation<List<Answer>> GetMany(Match match);
    }
}
