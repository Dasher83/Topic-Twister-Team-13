using System;
using TMPro;
using TopicTwister.NewRound.Presenters;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.NewRound.Views
{
    public class ResumeMatchView : MonoBehaviour, IResumeMatchView
    {
        public event Action<MatchDto> OnLoad;

        [SerializeField]
        private TextMeshProUGUI _roundNumberDisplay;

        [SerializeField]
        private Transform _categoryListRoot;
        
        [SerializeField]
        private GameObject _initialLetterButtonContainer;
        
        [SerializeField]
        private LoadSceneEventScriptable _loadSceneEventContainer;

        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        private Button _initialLetterButton;
        private TextMeshProUGUI __initialLetterText;
        private IResumeMatchPresenter _presenter;

        private void Start()
        {
            _presenter = new ResumeMatchPresenter(this);
            OnLoad?.Invoke(_matchCacheData.MatchDto);
            _initialLetterButton = _initialLetterButtonContainer.GetComponentInChildren<Button>();
            __initialLetterText = _initialLetterButtonContainer.GetComponentInChildren<TextMeshProUGUI>();
            _initialLetterButton.onClick.AddListener(() => InitialLetterRevealed());
        }

        private void InitialLetterRevealed()
        {
            __initialLetterText.text = _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString().ToUpper();
            _loadSceneEventContainer.LoadSceneWithDelay?.Invoke(Scenes.PlayTurn, 2f);
            _initialLetterButton.enabled = false;
        }

        public void UpdateMatchData(RoundWithCategoriesDto roundWithCategoriesDto)
        {
            _matchCacheData.RoundWithCategoriesDto = roundWithCategoriesDto;
            _roundNumberDisplay.text = $"Ronda {_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber + 1}";
            for(int i = 0; i < _categoryListRoot.childCount; i++)
            {
                _categoryListRoot.GetChild(i)
                    .GetComponentInChildren<TextMeshProUGUI>()
                    .text = _matchCacheData.RoundWithCategoriesDto.CategoryDtos[i].Name;
            }
        }
    }
}
