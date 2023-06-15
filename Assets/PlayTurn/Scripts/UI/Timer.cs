using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;
using System;
using TopicTwister.Shared.Constants;


namespace TopicTwister.PlayTurn.UI
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private float _numericTime;
        private bool _invokedTimedOut;
        private DateTime _initialDateTime;
        private TimeSpan _elapsedTime;
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
            _invokedTimedOut = false;
            if (!_interruptTurnEventContainer.isSubscribedTimer)
            {
                _interruptTurnEventContainer.isSubscribedTimer = true;
                _interruptTurnEventContainer.InterruptTurn += InterruptTurnEventHandler;
            }
            _initialDateTime = DateTime.UtcNow;
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
            SubstractTime();
        }

        private void SubstractTime()
        {
            _elapsedTime = DateTime.UtcNow.Subtract(_initialDateTime);

            if (_elapsedTime.TotalSeconds > Configuration.TurnDurationInSeconds && !_invokedTimedOut)
            {
                NumericTime = 0f;
                _timeOutEventContainer.TimeOut?.Invoke();
                _invokedTimedOut = true;
            }
            else if (!_invokedTimedOut)
            {
                NumericTime = Configuration.TurnDurationInSeconds - (float)_elapsedTime.TotalSeconds;
            }
        }

        private void InterruptTurnEventHandler()
        {
            NumericTime = 0f;
            _invokedTimedOut = true;
        }

        private void UpdateTimerText() => _timerText.text = string.Format("{0:0}", _numericTime);
    }
}
