using UnityEngine;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine.UI;
using TopicTwister.Shared.DTOs;
using System.Collections.Generic;
using System.Linq;


namespace TopicTwister.RoundResults
{
    public class LoadRoundResults : MonoBehaviour
    {
        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private Transform _categoryResultList;

        [SerializeField]
        private Transform _header;

        void Start()
        {
            LoadHeader();
            LoadPlayersAnswersComparison();
        }

        private void LoadHeader()
        {
            _header.Find("InitialLetter").GetComponentInChildren<TextMeshProUGUI>()
                .text = _matchCacheData.RoundWithCategoriesDto.RoundDto.InitialLetter.ToString().ToUpper();

            _header.Find("Title").GetComponentInChildren<TextMeshProUGUI>()
                .text = $"Final de ronda {_matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber + 1}";
        }

        private void LoadPlayersAnswersComparison()
        {
            List<AnswerDto> answerDtosOfUserWithInitiative = _matchCacheData
                .EndOfTurnDto
                .AnswerDtosOfUserWithIniciative
                .OrderBy(answerDto => answerDto.Order)
                .ToList();

            List<AnswerDto> answerDtosOfUserWithoutInitiative = _matchCacheData
               .EndOfTurnDto
               .AnswerDtosOfUserWithoutIniciative
               .OrderBy(answerDto => answerDto.Order)
               .ToList();

            int index = 0;
            foreach (Transform child in _categoryResultList)
            {
                child.Find("Category").GetComponent<TextMeshProUGUI>().text = _matchCacheData.RoundWithCategoriesDto
                    .CategoryDtos[index].Name;

                child.Find("AnswerPlayerOne")
                    .GetComponent<TextMeshProUGUI>().text = answerDtosOfUserWithInitiative[index].UserInput;

                child.Find("AnswerPlayerTwo")
                    .GetComponent<TextMeshProUGUI>().text = answerDtosOfUserWithoutInitiative[index].UserInput;

                if (answerDtosOfUserWithInitiative[index].IsCorrect)
                {
                    child.Find("ResultPlayerOne").GetComponent<Image>().sprite = _answerImageResultReferences.correctAnswer;
                }
                else
                {
                    child.Find("ResultPlayerOne").GetComponent<Image>().sprite = _answerImageResultReferences.incorrectAnswer;
                }

                if (answerDtosOfUserWithoutInitiative[index].IsCorrect)
                {
                    child.Find("ResultPlayerTwo").GetComponent<Image>().sprite = _answerImageResultReferences.correctAnswer;
                }
                else
                {
                    child.Find("ResultPlayerTwo").GetComponent<Image>().sprite = _answerImageResultReferences.incorrectAnswer;
                }

                index++;
            }
        }
    }
}
