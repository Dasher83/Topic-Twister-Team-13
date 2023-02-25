using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared;
using TopicTwister.NewRound.Actions;
using TopicTwister.Shared.Providers;
using TMPro;
using UnityEngine;
using TopicTwister.NewRound.Presenters;

namespace TopicTwister.NewRound.Views
{
    public class CategoriesListView : MonoBehaviour, ICategoriesListView
    {
        private ICategoryPresenter _categoryPresenter;

        private void Start()
        {
            _categoryPresenter = new CategoryPresenter(this);
            GetRandomCategoriesAction action = new ActionProvider<GetRandomCategoriesAction>().Provide();

            action.CategoryPresenter = _categoryPresenter;
            _categoryPresenter.Action = action;
            _categoryPresenter.GetRandomCategories(Constants.Categories.CategoriesPerRound);
        }

        public void UpdateCategoriesList(CategoryDTO[] categories)
        {
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = categories[i].Name;
            }
        }
    }
}
