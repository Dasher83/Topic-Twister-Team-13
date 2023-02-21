using TopicTwister.Backend.Shared.DTOs;
using TopicTwister.Backend.Shared.Interfaces;
using TMPro;
using TopicTwister.Backend.Presenters;
using UnityEngine;


namespace TopicTwister.Backend.Views
{
    public class BeginRoundView : MonoBehaviour
    {
        [SerializeField] private Transform _categoriesListObjectTransform;

        private CategoryDTO[] _categories;
        private ICategoryPresenter _categoryPresenter; 

        private void Start()
        {
            _categoryPresenter = new CategoryPresenterStub();
            
            _categories = _categoryPresenter.GetRandomCategories();
            for(int i = 0; i < _categoriesListObjectTransform.childCount; i++)
            {
                _categoriesListObjectTransform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = _categories[i].Name;
            }
        }
    }
}
