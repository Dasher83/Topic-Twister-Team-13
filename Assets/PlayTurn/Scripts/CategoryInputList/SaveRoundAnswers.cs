using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.DTOs;
using UnityEngine;

namespace TopicTwister.PlayTurn.CategoryInputList
{
    public class SaveRoundAnswers : MonoBehaviour
    {
        [SerializeField] private NewRoundScriptable _newRoundData;
        [SerializeField] private TurnAnswersScriptable _turnAnswersData;
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;
        [SerializeField] private LoadSceneEventScriptable _loadSceneEventContainer;

        private void Start()
        {
            _timeOutEventContainer.TimeOut += CaptureAndSaveDataEventHandler;
            _interruptTurnEventContainer.InterruptTurn += CaptureAndSaveDataEventHandler;
        }

        public void CaptureAndSaveDataEventHandler()
        {
            _turnAnswersData.ClearAnswers();

            TurnAnswerDTO[] turnAnswers = new TurnAnswerDTO[5];
            int index = 0;

            foreach (Transform childTransform in transform)
            {
                string userInput = childTransform.Find("UserInput").gameObject.GetComponent<TextMeshProUGUI>().text.Trim();
                turnAnswers[index] = new TurnAnswerDTO(
                    category: _newRoundData.Categories[index],
                    userInput: userInput,
                    order: index);
                index++;
            }

            _turnAnswersData.AddAnswers(turnAnswers);
            _loadSceneEventContainer.LoadSceneWithDelay(Scenes.TurnResultScene, 1f);
        }
    }
}
