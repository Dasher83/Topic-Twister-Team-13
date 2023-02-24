using TopicTwister.Backend.Shared.DTOs;


namespace TopicTwister.Backend.Shared.Interfaces
{
    public interface IGetNewRoundCategoriesUseCase
    {
        CategoryDTO[] GetRandomCategories();
    }
}
