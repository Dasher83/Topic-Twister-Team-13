using TopicTwister.Shared.Utils;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IWordsRepository
    {
        Operation<bool> Exists(string text, int categoryId, char initialLetter);
    }
}
