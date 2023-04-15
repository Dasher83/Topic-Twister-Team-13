using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.PlayTurn.Timer
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private float _numericTime;
        private bool _invokedTimedOut;
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
            _invokedTimedOut = false;
            _interruptTurnEventContainer.InterruptTurn += InterruptTurnEventHandler;
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
                _timeOutEventContainer.TimeOut?.Invoke();
                _invokedTimedOut = true;
            }
        }

        private void CountDown()
        {
            NumericTime -= Time.deltaTime;
        }
        
        private void InterruptTurnEventHandler()
        {
            NumericTime = 0;
            _invokedTimedOut = true;
        }

        private void UpdateTimerText() => _timerText.text = string.Format("{0:0}", _numericTime);
    }
}

