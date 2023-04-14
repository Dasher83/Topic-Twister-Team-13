using TopicTwister.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;


namespace TopicTwister.NewRound.Services
{
    public class CategoriesService : ICategoriesService
    {
        private IGetNewRoundCategoriesUseCase _useCase;

        
        private  CategoriesService(){}
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
}