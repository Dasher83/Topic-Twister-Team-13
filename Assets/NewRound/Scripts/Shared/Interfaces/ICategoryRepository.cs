using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICategoryRepository
    {
        CategoryDTO[] GetRandomCategories(int numberOfCategories);
        bool Exists(string name);
        bool Exists(string[] names);
    }
}
