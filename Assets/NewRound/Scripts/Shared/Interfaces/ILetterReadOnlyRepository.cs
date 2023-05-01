using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ILetterReadOnlyRepository
    {
        Result<char> GetRandomLetter();
    }
}
