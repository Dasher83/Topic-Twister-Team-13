using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;

public class CategoryRepositoryScriptable : ICategoryRepository
{
    public CategoryDTO[] GetRandomCategories(int numberOfCategories)
    {
        return new CategoryDTO[5];
    }
}
