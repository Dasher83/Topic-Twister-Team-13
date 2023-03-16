using TMPro;
using UnityEngine;


namespace TopicTwister.PlayRound.Scripts.Timer
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private float _numericTime;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
        }

        void Update()
        {
            CountDown();
        }

        private void CountDown()
        {
            if (_timerText.text == "0") return;

            _numericTime -= Time.deltaTime;
            _timerText.text = string.Format("{0:0}", _numericTime);
        }
    }
}

