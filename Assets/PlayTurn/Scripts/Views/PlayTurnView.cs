using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Constants;
using TopicTwister.PlayTurn.Presenters;
using System.Collections.Generic;
using TopicTwister.PlayTurn.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Views
{
    public class PlayTurnView : MonoBehaviour, IPlayTurnView
    {
        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private TextMeshProUGUI _roundNumber;

        [SerializeField]
        private TextMeshProUGUI _initialLetter;

        [SerializeField]
        private TimeOutEventScriptable _timeOutEventContainer;

        [SerializeField]
        private InterruptTurnEventScriptable _interruptTurnEventContainer;

        [SerializeField]
        private LoadSceneEventScriptable _loadSceneEventContainer;

        private List<TextMeshProUGUI> _userInputTexts;
        private Transform _categoryListRoot;
        public event EventDelegates.IPlayTurnView.LoadEventHandler Load;
        public event EventDelegates.IPlayTurnView.EndTurnEventHandler EndTurn;
        private IEndTurnPresenter _endTurnPresenter;

        private void Start()
        {
            new StartTurnPresenter(this);
            _endTurnPresenter = new EndTurnPresenter(this);
            _categoryListRoot = GameObject.Find("CategoryInputList").transform;
            _userInputTexts = new List<TextMeshProUGUI>();

            foreach (Transform child in _categoryListRoot)
            {
                _userInputTexts.Add(child.Find("UserInput").GetComponent<TextMeshProUGUI>());
            }

            LoadRoundData();
            if (!_interruptTurnEventContainer.isSubscribedPlayTurn)
            {
                _interruptTurnEventContainer.isSubscribedPlayTurn = true;
                _timeOutEventContainer.TimeOut += CaptureAndSaveDataEventHandler;
                _interruptTurnEventContainer.InterruptTurn += CaptureAndSaveDataEventHandler;
            }

            Load?.Invoke(
                userId: Configuration.TestUserId,
                matchId: _matchCacheData.MatchDto.Id);

            _matchCacheData.UserInputChanged += UpdateUserInputText;
        }

        private void LoadRoundData()
        {
            AnswerDraftDto[] answerDrafts = new AnswerDraftDto[_categoryListRoot.childCount];
            _roundNumber.text = $"Ronda {_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber + 1}";
            _initialLetter.text = _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString().ToUpper();
            GameObject child;

            for(int i = 0; i < _categoryListRoot.childCount; i++)
            {
                child = _categoryListRoot.GetChild(i).Find("Category").gameObject;
                child.GetComponent<TextMeshProUGUI>().text = _matchCacheData.RoundWithCategoriesDto.CategoryDtos[i].Name;
                answerDrafts[i] = new AnswerDraftDto(
                    category: _matchCacheData.RoundWithCategoriesDto.CategoryDtos[i],
                    order: i);
            }
            _matchCacheData.UpdateDraftAnswers(answerDrafts);
        }

        private void CaptureAndSaveDataEventHandler()
        {

            AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];
            int index = 0;

            foreach (TextMeshProUGUI userInputText in _userInputTexts)
            {
                string userInput = userInputText.text.Trim();

                answerDtos[index] = new AnswerDto(
                    categoryDto: _matchCacheData.RoundWithCategoriesDto.CategoryDtos[index],
                    userInput: _matchCacheData.TurnAnswerDrafts[index].UserInput,
                    order: index,
                    isCorrect: false);

                index++;
            }

            EndTurn?.Invoke(
                userId: Configuration.TestUserId,
                matchId: _matchCacheData.MatchDto.Id,
                answerDtos: answerDtos);
        }

        public void ReceiveUpdateFromStartTurn(TurnDto turnDto)
        {
            _matchCacheData.TurnDto = turnDto;
        }

        private void UpdateUserInputText(int index)
        {
            _userInputTexts[index].text = _matchCacheData.TurnAnswerDrafts[index].UserInput;
        }

        public void ReceiveUpdateFromEndTurn(EndOfTurnDto endOfTurnDto)
        {
            _matchCacheData.UpdateEndOfTurn(endOfTurnDto);

            _loadSceneEventContainer.LoadSceneWithDelay(
                Configuration.Scenes.TurnResultScene,
                Configuration.TransitionsDuration.FromPlayTurnToTurnResult);
        }
    }
}
