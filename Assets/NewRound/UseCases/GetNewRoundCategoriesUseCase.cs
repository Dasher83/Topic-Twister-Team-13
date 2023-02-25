using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.UseCases
{
    public class GetNewRoundCategoriesUseCase : IGetNewRoundCategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetNewRoundCategoriesUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryDTO[] GetRandomCategories(int numberOfCategories)
        {
            return _categoryRepository.GetRandomCategories(numberOfCategories);
        }
    }
}
