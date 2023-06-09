using TopicTwister.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared;
using TopicTwister.NewRound.Presenters;
using TMPro;
using UnityEngine;
using TopicTwister.Shared.ScriptableObjects;


namespace TopicTwister.NewRound.Views
{
    public class CategoriesListView : MonoBehaviour, ICategoriesListView
    {
        [SerializeField]
        private NewRoundScriptable _newRoundData;
        private ICategoryPresenter _categoryPresenter;

        private void Start()
        {
            _categoryPresenter = new CategoryPresenter(this);
            _categoryPresenter.GetRandomCategories(Constants.Categories.CategoriesPerRound);
        }

        public void UpdateCategoriesList(CategoryDTO[] categories)
        {
            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                this.gameObject.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = categories[i].Name;
            }
            _newRoundData.Initialize(categories);
        }
    }
}
