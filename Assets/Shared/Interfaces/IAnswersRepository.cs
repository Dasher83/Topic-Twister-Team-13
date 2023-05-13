using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IAnswersRepository : IAnswersReadOnlyRepository
    {
        Operation<Answer> Insert(Answer answer);
    }
}
