using UnityEngine;
using TopicTwister.Shared.ScriptableObjects.FakeMatch;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine.UI;


namespace TopicTwister.RoundResults
{
    public class LoadRoundResults : MonoBehaviour
    {
        [SerializeField]
        private FakeMatchScriptable _fakeMatchData;

        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        [SerializeField]
        private AnswerImageResultScriptable _answerImageResultReferences;

        [SerializeField]
        private Transform _cateoryResultList;

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
            int index = 0;
            foreach (Transform child in _cateoryResultList)
            {
                FakeMatchScriptable.FakeRound fakeRound = _fakeMatchData.Rounds[
                    _matchCacheData.RoundWithCategoriesDto.RoundDto.RoundNumber];

                child.Find("Category").GetComponent<TextMeshProUGUI>().text = _matchCacheData.RoundWithCategoriesDto
                    .CategoryDtos[index].Name;

                child.Find("AnswerPlayerOne").GetComponent<TextMeshProUGUI>().text = fakeRound.UserAnswer[index].Answer;

                child.Find("AnswerPlayerTwo").GetComponent<TextMeshProUGUI>().text = fakeRound.BotAnswer[index].Answer;

                if (fakeRound.UserAnswer[index].IsCorrect)
                {
                    child.Find("ResultPlayerOne").GetComponent<Image>().sprite = _answerImageResultReferences.correctAnswer;
                }
                else
                {
                    child.Find("ResultPlayerOne").GetComponent<Image>().sprite = _answerImageResultReferences.incorrectAnswer;
                }

                if (fakeRound.BotAnswer[index].IsCorrect)
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
