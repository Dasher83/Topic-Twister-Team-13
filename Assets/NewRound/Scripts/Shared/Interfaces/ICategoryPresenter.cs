using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface ICategoryPresenter
    {
        void GetRandomCategories(int numberOfCategories);
        void UpdateCategoriesList(CategoryDTO[] categories);
        IAction Action { set; }
    }
}
