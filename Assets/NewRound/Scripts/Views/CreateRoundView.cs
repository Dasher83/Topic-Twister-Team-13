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
    public class CreateRoundView : MonoBehaviour, ICreateRoundView
    {
        public event EventHandler OnLoad;

        [SerializeField]
        private RoundCacheScriptable _newRoundData;

        [SerializeField]
        private TextMeshProUGUI _roundNumberDisplay;

        [SerializeField]
        private Transform _categoryListRoot;
        
        [SerializeField]
        private GameObject _initialLetterButtonContainer;
        
        [SerializeField]
        private LoadSceneEventScriptable _loadSceneEventContainer;

        private Button _initialLetterButton;
        private TextMeshProUGUI __initialLetterText;
        private ICreateRoundPresenter _presenter;

        private void Start()
        {
            _presenter = new CreateRoundPresenter(this);
            OnLoad?.Invoke(this, EventArgs.Empty);
            _initialLetterButton = _initialLetterButtonContainer.GetComponentInChildren<Button>();
            __initialLetterText = _initialLetterButtonContainer.GetComponentInChildren<TextMeshProUGUI>();
            _initialLetterButton.onClick.AddListener(() => InitialLetterRevealed());
            _roundNumberDisplay.text = $"Ronda {_newRoundData.RoundDto.RoundNumber}";
        }

        private void InitialLetterRevealed()
        {
            __initialLetterText.text = _newRoundData.RoundDto.InitialLetter.ToString();
            _loadSceneEventContainer.LoadSceneWithDelay?.Invoke(Scenes.PlayTurn, 2f);
            _initialLetterButton.enabled = false;
        }

        public void UpdateNewRoundData(RoundWithCategoriesDto roundWithCategoriesDto)
        {
            _newRoundData.Initialize(
                roundDto: roundWithCategoriesDto.RoundDto,
                categoryDtos: roundWithCategoriesDto.CategoryDtos);
        }
    }
}
