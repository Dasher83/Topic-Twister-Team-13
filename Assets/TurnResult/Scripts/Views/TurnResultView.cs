using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TopicTwister.ResultRound.Presenters;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.Structs;
using TopicTwister.ResultRound.Shared.Interfaces;
using TopicTwister.ResultRound.Shared.Structs;
using UnityEngine.UI;


namespace TopicTwister.ResultRound.Views
{
    public class TurnResultView : MonoBehaviour, IResultRoundView
    {
        public event Action OnLoad;

        [SerializeField]
        private Transform categoryResultList;

        [SerializeField]
        private RoundAnswersScriptable roundAnswer;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private char _initialLetter; //TODO get initial letter from scene
        
        private List<RoundAnswer> _resultRoundViewList;
        private ResultRoundPresenter _resultRoundPresenter;
        private Sprite _answerResultImage;

        void Start()
        {
            LoadCategoryResultList();
            _resultRoundPresenter = new ResultRoundPresenter(resultRoundView: this);
            OnLoad?.Invoke();
        }

        public void EvaluateAnswers(List<EvaluatedAnswerStruct> evaluatedAnswers)
        {
            for (int i = 0; i < categoryResultList.childCount; i++)
            {
                foreach (EvaluatedAnswerStruct evaluatedAnswer in evaluatedAnswers)
                {
                    if (categoryResultList.transform.GetChild(i).Find("Category").gameObject.GetComponent<TextMeshProUGUI>().text == evaluatedAnswer.category)
                    {
                        if (evaluatedAnswer.isCorrect)
                        {
                            _answerResultImage = _answerImageResultReferences.correctAnswer;
                        }
                        else
                        {
                            _answerResultImage = _answerImageResultReferences.incorrectAnswer;
                        }
                        categoryResultList.transform.GetChild(i).Find("Result").gameObject.GetComponent<Image>().sprite = _answerResultImage;
                    }
                }
            }
        }

        public AnswersToEvaluateStruct GetAnswersToEvaluate()
        {
            return new AnswersToEvaluateStruct(_initialLetter, _resultRoundViewList);
        }

        public void LoadCategoryResultList()
        {
            _resultRoundViewList = roundAnswer.GetRoundAnswers();

            for(int i = 0; i < categoryResultList.childCount; i++)
            {
                categoryResultList.transform.GetChild(i).Find("Category").gameObject.GetComponent<TextMeshProUGUI>().text = _resultRoundViewList[i].CategoryId;
                categoryResultList.transform.GetChild(i).Find("Answer").gameObject.GetComponent<TextMeshProUGUI>().text = _resultRoundViewList[i].UserInput;
            }
        }
    }
}
