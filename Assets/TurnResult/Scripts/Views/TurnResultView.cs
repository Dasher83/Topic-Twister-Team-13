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
        public event Action OnLoad;

        [SerializeField]
        private Transform _categoryResultList;

        [SerializeField]
        private TextMeshProUGUI _initialLetterDisplay;

        [SerializeField]
        private TurnAnswersScriptable _turnAnswer;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private NewRoundScriptable _newRoundData;

        private char _initialLetter;

        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;

        [SerializeField]
        private FakeMatchScriptable _fakeMatchData;

        private List<TurnAnswerDTO> _turnResultViewList;
        private TurnResultPresenter _turnResultPresenter;
        private Sprite _answerResultImage;
        private EvaluatedAnswerDTO _evaluatedAnswer;

        void Start()
        {
            _initialLetter = _newRoundData.InitialLetter;
            _initialLetterDisplay.text = _initialLetter.ToString();
            LoadCategoryResultList();
            _turnResultPresenter = new TurnResultPresenter(turnResultView: this);
            OnLoad?.Invoke();
        }

        public void EvaluateAnswers(List<EvaluatedAnswerDTO> evaluatedAnswers)
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
                roundNumber: _newRoundData.RoundNumber,
                initialLetter: _newRoundData.InitialLetter.ToString());
        }

        public AnswersToEvaluateDTO GetAnswersToEvaluate()
        {
            return new AnswersToEvaluateDTO(_initialLetter, _turnResultViewList);
        }

        public void LoadCategoryResultList()
        {
            _turnResultViewList = _turnAnswer.GetTurnAnswers();

            for(int i = 0; i < _categoryResultList.childCount; i++)
            {
                _categoryResultList.transform.GetChild(i).Find("Category")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = _turnResultViewList[i].Category.Name;
                _categoryResultList.transform.GetChild(i).Find("Answer")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = _turnResultViewList[i].UserInput;
            }
        }

        public void FinishTurnReview()
        {
            _eventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.BeginRoundScene);
        }
    }
}
