using TopicTwister.NewRound.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICategoryRepository
    {
        CategoryDTO[] GetRandomCategories(int numberOfCategories);
    }
}
