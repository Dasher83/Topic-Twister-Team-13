using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ILetterRepository
    {
        Result<char> GetRandomLetter();
    }
}
