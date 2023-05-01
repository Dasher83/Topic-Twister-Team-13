using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Repositories
{
    public class LetterReadOnlyRepositoryInMemory : ILetterReadOnlyRepository
    {
        public Result<char> GetRandomLetter()
        {
            throw new System.NotImplementedException();
        }
    }
}
