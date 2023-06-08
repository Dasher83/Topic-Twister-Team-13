using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.DTOs;
using UnityEngine.UI;
using TopicTwister.Shared.Constants;
using System.Linq;


namespace TopicTwister.TurnResult.Views
{
    public class TurnResultView : MonoBehaviour
    {
        [SerializeField]
        private Transform _categoryResultList;

        [SerializeField]
        private Transform _header;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private LoadSceneEventScriptable _eventContainer;

        void Start()
        {
            _header.Find("InitialLetter").GetComponentInChildren<TextMeshProUGUI>()
                .text = _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString().ToUpper();
            _header.Find("Round").GetComponentInChildren<TextMeshProUGUI>()
                .text = $"Ronda {_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber + 1}";
            LoadCategoryResultList();
        }

        public void LoadCategoryResultList()
        {
            List<AnswerDto> answerDtos = _matchCacheData
                .EndOfTurnDto
                .AnswerDtosOfUserWithIniciative
                .OrderBy(answerDto => answerDto.Order)
                .ToList();

            Sprite answerResultImage;

            for (int i = 0; i < _categoryResultList.childCount; i++)
            {
                _categoryResultList.transform.GetChild(i).Find("Category")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = answerDtos[i].CategoryDto.Name;
                _categoryResultList.transform.GetChild(i).Find("Answer")
                    .gameObject.GetComponent<TextMeshProUGUI>().text = answerDtos[i].UserInput;

                if (answerDtos[i].IsCorrect)
                {
                    answerResultImage = _answerImageResultReferences.correctAnswer;
                }
                else
                {
                    answerResultImage = _answerImageResultReferences.incorrectAnswer;
                }

                _categoryResultList.transform.GetChild(i).Find("Result")
                        .gameObject.GetComponent<Image>().sprite = answerResultImage;
            }
        }

        public void FinishTurnReview()
        {
            //TODO : decidir a que escena ir según el estado de endTurnDto
            _eventContainer.LoadSceneWithoutDelay?.Invoke(Configuration.Scenes.RoundResults);
        }
    }
}
