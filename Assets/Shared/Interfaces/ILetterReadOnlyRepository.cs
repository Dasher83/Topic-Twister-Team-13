using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface ILetterReadOnlyRepository
    {
        Operation<char> GetRandomLetter();
    }
}
