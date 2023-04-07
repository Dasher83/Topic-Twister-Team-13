namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IWordsRepository
    {
        bool Exists(string text, string categoryId, char initialLetter);
    }
}
