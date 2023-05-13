using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IAnswersReadOnlyRepository
    {
        Operation<List<Answer>> GetAll();
        Operation<Answer> Get(int userId, int roundId, int categoryId);
    }
}
