using TMPro;
using TopicTwister.Shared.Structs;
using UnityEngine;

namespace TopicTwister.PlayRound.Scripts.CategoryInputList
{
    public class SaveRoundAnswers : MonoBehaviour
    {
        [SerializeField] private RoundAnswersScriptable _roundAnswersData;
        [SerializeField] private Timer.Timer _currentTimer;
        [SerializeField] private StopButton.StopButton _stopButton;

        private void Start()
        {
            _currentTimer.timedOut.AddListener(CaptureAndSaveDataEventHandler);
            _stopButton.InterruptRound.AddListener(CaptureAndSaveDataEventHandler);
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
                roundAnswers[index] = new RoundAnswer(categoryId, userInput);
                index++;
            }

            _roundAnswersData.AddAnswers(roundAnswers);
        }
    }
}
