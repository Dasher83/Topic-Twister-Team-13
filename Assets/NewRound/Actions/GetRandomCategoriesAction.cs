using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Actions
{
    public class GetRandomCategoriesAction : IAction
    {
        private ICategoryPresenter _categoryPresenter;
        private ICategoriesService _categoriesService;

        private GetRandomCategoriesAction(){}
        
        public GetRandomCategoriesAction(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public ICategoryPresenter CategoryPresenter { set { _categoryPresenter = value; } }

        public void Execute()
        {
            CategoryDTO[] categories = _categoriesService.GetRandomCategories(Constants.Categories.CategoriesPerRound);
            _categoryPresenter.UpdateCategoriesList(categories: categories);
        }
    }
}
