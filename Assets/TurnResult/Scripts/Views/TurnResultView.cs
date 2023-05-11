using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TopicTwister.TurnResult.Presenters;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using UnityEngine.UI;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;


namespace TopicTwister.TurnResult.Views
{
    public class TurnResultView : MonoBehaviour, ITurnResultView
    {
        public event EventDelegates.ITurnResultView.LoadEventHandler Load;

        [SerializeField]
        private Transform _categoryResultList;

        [SerializeField]
        private Transform _header;

        [SerializeField]
        private TurnAnswersScriptable _turnAnswer;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;

        [SerializeField]
        private FakeMatchScriptable _fakeMatchData;

        private List<AnswerDto> _turnResultViewList;
        private Sprite _answerResultImage;
        private EvaluatedAnswerDto _evaluatedAnswer;

        void Start()
        {
            _header.Find("InitialLetter").GetComponentInChildren<TextMeshProUGUI>()
                .text = _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString().ToUpper();
            _header.Find("Round").GetComponentInChildren<TextMeshProUGUI>()
                .text = $"Ronda {_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber + 1}";
            LoadCategoryResultList();
            new TurnResultPresenter(turnResultView: this);
            Load?.Invoke();
        }

        public void EvaluateAnswers(List<EvaluatedAnswerDto> evaluatedAnswers)
        {
            for (int i = 0; i < _categoryResultList.childCount; i++)
            {
                _evaluatedAnswer = evaluatedAnswers.Find(evaluatedAnswer => evaluatedAnswer.Order == i);

                if (_categoryResultList.transform.GetChild(i).Find("Category")
                    .gameObject.GetComponent<TextMeshProUGUI>().text == _evaluatedAnswer.Category.Name)
                {
                    if (_evaluatedAnswer.IsCorrect)
                    {
                        _answerResultImage = _answerImageResultReferences.correctAnswer;
                    }
                    else
                    {
                        _answerResultImage = _answerImageResultReferences.incorrectAnswer;
                    }
                    _categoryResultList.transform.GetChild(i).Find("Result")
                        .gameObject.GetComponent<Image>().sprite = _answerResultImage;
                }
            }
            _fakeMatchData.AddRound(
                userAnswers: evaluatedAnswers,
                initialLetter: _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString());
        }

        public AnswersToEvaluateDTO GetAnswersToEvaluate()
        {
            return new AnswersToEvaluateDTO(
                _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter,
                _turnResultViewList);
        }

        public void LoadCategoryResultList()
        {
            _turnResultViewList = _turnAnswer.GetTurnAnswers();

            for(int i = 0; i < _categoryResultList.childCount; i++)
            {
                _categoryResultList.transform.GetChild(i).Find("Category")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = _turnResultViewList[i].CategoryDto.Name;
                _categoryResultList.transform.GetChild(i).Find("Answer")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = _turnResultViewList[i].UserInput;
            }
        }

        public void FinishTurnReview()
        {
            _eventContainer.LoadSceneWithoutDelay?.Invoke(Configuration.Scenes.RoundResults);
        }
    }
}
