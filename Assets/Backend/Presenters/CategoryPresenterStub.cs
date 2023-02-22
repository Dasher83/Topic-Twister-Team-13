using TopicTwister.Backend.Shared.DTOs;
using TopicTwister.Backend.Shared.Interfaces;


namespace TopicTwister.Backend.Presenters
{
    public class CategoryPresenterStub : ICategoryPresenter
    {
        private readonly IGetNewRoundCategoriesUseCase _newRoundCategoriesUseCase;

        private CategoryPresenterStub()
        {
            
        }

        public CategoryPresenterStub(IGetNewRoundCategoriesUseCase newRoundCategoriesUseCase)
        {
            this._newRoundCategoriesUseCase = newRoundCategoriesUseCase;
        }
        
        
        public CategoryDTO[] GetRandomCategories()
        {
            return _newRoundCategoriesUseCase.GetRandomCategories();
        }
    }
}
