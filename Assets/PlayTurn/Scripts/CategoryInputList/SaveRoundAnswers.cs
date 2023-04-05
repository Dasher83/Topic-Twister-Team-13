using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.ScriptableObjects;
using TopicTwister.Shared.Structs;
using UnityEngine;

namespace TopicTwister.PlayTurn.CategoryInputList
{
    public class SaveRoundAnswers : MonoBehaviour
    {
        [SerializeField] private RoundAnswersScriptable _roundAnswersData;
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
            _roundAnswersData.ClearAnswers();

            RoundAnswer[] roundAnswers = new RoundAnswer[5];
            int index = 0;

            foreach (Transform childTransform in transform)
            {
                string categoryId = childTransform.Find("Category").gameObject.GetComponent<TextMeshProUGUI>().text;
                string userInput = childTransform.Find("UserInput").gameObject.GetComponent<TextMeshProUGUI>().text;
                roundAnswers[index] = new RoundAnswer(categoryId, userInput, order: index);
                index++;
            }

            _roundAnswersData.AddAnswers(roundAnswers);
            _loadSceneEventContainer.LoadSceneWithDelay(Scenes.TurnResultScene, 1f);
        }
    }
}
