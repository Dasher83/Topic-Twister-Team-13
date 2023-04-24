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
        private RoundCacheScriptable _roundCache;

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
                .text = _roundCache.RoundDto.InitialLetter.ToString();

            _header.Find("Title").GetComponentInChildren<TextMeshProUGUI>()
                .text = $"Final de ronda {_roundCache.RoundDto.RoundNumber}";
        }

        private void LoadPlayersAnswersComparison()
        {
            int index = 0;
            foreach (Transform child in _cateoryResultList)
            {

                child.Find("Category").GetComponent<TextMeshProUGUI>().text = _roundCache.Categories[index].Name;

                child.Find("AnswerPlayerOne").GetComponent<TextMeshProUGUI>()
                    .text = _fakeMatchData.Rounds[_roundCache.RoundDto.RoundNumber - 1].UserAnswer[index].Answer;

                child.Find("AnswerPlayerTwo").GetComponent<TextMeshProUGUI>()
                    .text = _fakeMatchData.Rounds[_roundCache.RoundDto.RoundNumber - 1].BotAnswer[index].Answer;

                if (_fakeMatchData.Rounds[_roundCache.RoundDto.RoundNumber - 1].UserAnswer[index].IsCorrect)
                {
                    child.Find("ResultPlayerOne").GetComponent<Image>().sprite = _answerImageResultReferences.correctAnswer;
                }
                else
                {
                    child.Find("ResultPlayerOne").GetComponent<Image>().sprite = _answerImageResultReferences.incorrectAnswer;
                }

                if (_fakeMatchData.Rounds[_roundCache.RoundDto.RoundNumber - 1].BotAnswer[index].IsCorrect)
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
