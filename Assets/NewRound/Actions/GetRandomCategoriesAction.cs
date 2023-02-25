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

        public GetRandomCategoriesAction(ICategoryPresenter categoryPresenter, ICategoriesService categoriesService)
        {
            _categoryPresenter = categoryPresenter;
            _categoriesService = categoriesService;
        }

        public void Execute()
        {
            CategoryDTO[] categories = _categoriesService.GetRandomCategories(Constants.Categories.CategoriesPerRound);
            _categoryPresenter.UpdateCategoriesList(categories: categories);
        }
    }
}
