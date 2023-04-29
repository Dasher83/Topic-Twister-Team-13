using TopicTwister.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IGetNewRoundCategoriesUseCase
    {
        CategoryDto[] GetRandomCategories(int numberOfCategories);
    }
}
