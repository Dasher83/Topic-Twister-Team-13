using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;


public class CategoriesService : ICategoriesService
{
    private IGetNewRoundCategoriesUseCase _useCase;

    public CategoriesService(IGetNewRoundCategoriesUseCase useCase)
    {
        _useCase = useCase;
    }

    public CategoryDTO[] GetRandomCategories(int numberOfCategories)
    {
        CategoryDTO[] categories = _useCase.GetRandomCategories(numberOfCategories);
        return categories;
    }
}
