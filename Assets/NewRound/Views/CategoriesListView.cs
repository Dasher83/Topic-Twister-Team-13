using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Presenters;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared;
using TMPro;
using UnityEngine;
using TopicTwister.NewRound.Actions;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Views
{
    public class CategoriesListView : MonoBehaviour, ICategoriesListView
    {
        [SerializeField] private Transform _categoriesListObjectTransform;

        private ICategoryPresenter _categoryPresenter;

        private void Start()
        {
            IGetNewRoundCategoriesUseCase useCase = new GetNewRoundCategoriesUseCase(
                categoryRepository: new CategoryRepositoryStub());

            _categoryPresenter = new CategoryPresenter(categoriesListView: this);

            IAction getRandomCategoriesAction = new GetRandomCategoriesAction(
                categoryPresenter: _categoryPresenter,
                categoriesService: new CategoriesService(useCase));

            _categoryPresenter.Action = getRandomCategoriesAction;

            _categoryPresenter.GetRandomCategories(Constants.Categories.CategoriesPerRound);
        }

        public void UpdateCategoriesList(CategoryDTO[] categories)
        {
            for (int i = 0; i < _categoriesListObjectTransform.childCount; i++)
            {
                _categoriesListObjectTransform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = categories[i].Name;
            }
        }
    }
}
