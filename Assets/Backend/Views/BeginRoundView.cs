using TopicTwister.Backend.Shared.DTOs;
using TopicTwister.Backend.Shared.Interfaces;
using TMPro;
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
            /* 
             * Inyect dependency into _categoryPresenter here or in Awake method, 
             * once a concrete class that realizes the ICategoryPresenter is implemented
             */
            _categories = _categoryPresenter.GetRandomCategories();
            for(int i = 0; i < _categoriesListObjectTransform.childCount; i++)
            {
                _categoriesListObjectTransform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = _categories[i].Name;
            }
        }
    }
}
