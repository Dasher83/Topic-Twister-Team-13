using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.NewRound.Actions;
using TopicTwister.Shared.Providers;


namespace TopicTwister.NewRound.Presenters
{
    public class CategoryPresenter : ICategoryPresenter
    {
        private ICategoriesListView _categoriesListView;
        private IAction _getRandomCategoriesAction; 

        private CategoryPresenter() { }

        public CategoryPresenter(ICategoriesListView categoriesListView)
        {
            _categoriesListView = categoriesListView;
            GetRandomCategoriesAction action = new ActionProvider<GetRandomCategoriesAction>().Provide();
            action.CategoryPresenter = this;
            this.Action = action;
        }
        
        public IAction Action { set { _getRandomCategoriesAction = value; } }

        public void GetRandomCategories(int numberOfCategories)
        {
            _getRandomCategoriesAction.Execute();
        }

        public void UpdateCategoriesList(CategoryDTO[] categories)
        {
            _categoriesListView.UpdateCategoriesList(categories);
        }
    }
}
