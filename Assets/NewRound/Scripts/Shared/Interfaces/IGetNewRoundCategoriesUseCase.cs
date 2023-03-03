using TopicTwister.NewRound.Shared.DTOs;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IGetNewRoundCategoriesUseCase
    {
        CategoryDTO[] GetRandomCategories(int numberOfCategories);
    }
}
