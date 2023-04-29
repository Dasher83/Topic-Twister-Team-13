using TopicTwister.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.UseCases
{
    public class GetNewRoundCategoriesUseCase : IGetNewRoundCategoriesUseCase
    {
        private readonly ICategoriesReadOnlyRepository _categoryRepository;

        public GetNewRoundCategoriesUseCase(ICategoriesReadOnlyRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryDto[] GetRandomCategories(int numberOfCategories)
        {
            return _categoryRepository.GetRandomCategories(numberOfCategories);
        }
    }
}
