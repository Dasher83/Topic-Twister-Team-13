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
        [SerializeField] private StopButton.StopButton stopButton;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
            _invokedTimedOut = false;
            stopButton.InterruptRound.AddListener(InterruptRoundEventHandler);
        }

        private float NumericTime
        {
            get
            {
                return _numericTime;
            }

            set
            {
                _numericTime = value;
                UpdateTimerText();
            }
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
            NumericTime -= Time.deltaTime;
        }
        
        private void InterruptRoundEventHandler()
        {
            NumericTime = 0;
            _invokedTimedOut = true;
        }

        private void UpdateTimerText() => _timerText.text = string.Format("{0:0}", _numericTime);
    }
}

