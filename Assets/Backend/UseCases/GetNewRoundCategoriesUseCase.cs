using TopicTwister.Backend.Shared.DTOs;
using TopicTwister.Backend.Shared.Interfaces;


namespace TopicTwister.Backend.UseCases
{
    public class GetNewRoundCategoriesUseCase : IGetNewRoundCategoriesUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetNewRoundCategoriesUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public CategoryDTO[] GetRandomCategories()
        {
            return _categoryRepository.GetRandomCategories();
        }
    }
}
