using TopicTwister.Shared.Utils;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IWordsRepository
    {
        Result<bool> Exists(string text, int categoryId, char initialLetter);
    }
}
