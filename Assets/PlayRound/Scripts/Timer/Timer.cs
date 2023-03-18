using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace TopicTwister.PlayRound.Scripts.Timer
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private float _numericTime;
        public UnityEvent timedOut = new UnityEvent();
        private bool _invokedTimedOut;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
            _invokedTimedOut = false;
        }

        void Update()
        {
            if (_timerText.text != "0")
            {
                CountDown();
            }
            else if (!_invokedTimedOut)
            {
                timedOut.Invoke();
                _invokedTimedOut = true;
            }
        }

        private void CountDown()
        {
            _numericTime -= Time.deltaTime;
            _timerText.text = string.Format("{0:0}", _numericTime);
        }
        
        
    }
}

