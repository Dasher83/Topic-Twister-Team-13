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


namespace TopicTwister.TurnResult.Views
{
    public class TurnResultView : MonoBehaviour, IResultRoundView
    {
        public event Action OnLoad;

        [SerializeField]
        private Transform _categoryResultList;

        [SerializeField]
        private TextMeshProUGUI _initialLetterDisplay;

        [SerializeField]
        private RoundAnswersScriptable roundAnswer;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private NewRoundScriptable _newRoundData;

        private char _initialLetter;

        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;
        
        private List<TurnAnswerDTO> _resultRoundViewList;
        private ResultRoundPresenter _resultRoundPresenter;
        private Sprite _answerResultImage;
        private EvaluatedAnswerDTO _evaluatedAnswer;

        void Start()
        {
            _initialLetter = _newRoundData.InitialLetter;
            _initialLetterDisplay.text = _initialLetter.ToString();
            LoadCategoryResultList();
            _resultRoundPresenter = new ResultRoundPresenter(resultRoundView: this);
            OnLoad?.Invoke();
        }

        public void EvaluateAnswers(List<EvaluatedAnswerDTO> evaluatedAnswers)
        {
            for (int i = 0; i < _categoryResultList.childCount; i++)
            {
                _evaluatedAnswer = evaluatedAnswers.Find(evaluatedAnswer => evaluatedAnswer.order == i);

                if (_categoryResultList.transform.GetChild(i).Find("Category")
                    .gameObject.GetComponent<TextMeshProUGUI>().text == _evaluatedAnswer.category.Name)
                {
                    if (_evaluatedAnswer.isCorrect)
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
        }

        public AnswersToEvaluateDTO GetAnswersToEvaluate()
        {
            return new AnswersToEvaluateDTO(_initialLetter, _resultRoundViewList);
        }

        public void LoadCategoryResultList()
        {
            _resultRoundViewList = roundAnswer.GetRoundAnswers();

            for(int i = 0; i < _categoryResultList.childCount; i++)
            {
                _categoryResultList.transform.GetChild(i).Find("Category")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = _resultRoundViewList[i].Category.Name;
                _categoryResultList.transform.GetChild(i).Find("Answer")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = _resultRoundViewList[i].UserInput;
            }
        }

        public void FinishTurnReview()
        {
            _eventContainer.LoadSceneWithoutDelay?.Invoke(Scenes.BeginRoundScene);
        }
    }
}
