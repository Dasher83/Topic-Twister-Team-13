using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;
using System;


namespace TopicTwister.PlayTurn.Timer
{
    public class Timer : MonoBehaviour
    {
        private TextMeshProUGUI _timerText;
        private float _numericTime;
        private bool _invokedTimedOut;
        private DateTime _initialDateTime;
        private TimeSpan _elapsedTime;
        private const float TurnMaxDuration = 60f;
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
            _invokedTimedOut = false;
            _interruptTurnEventContainer.InterruptTurn += InterruptTurnEventHandler;
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
            DifferenceTimes();
        }


        
        private void DifferenceTimes()
        {
            _elapsedTime = DateTime.UtcNow.Subtract(_initialDateTime);

            if (_elapsedTime.TotalSeconds > TurnMaxDuration && !_invokedTimedOut) 
            {
                NumericTime = 0f;
                _timeOutEventContainer.TimeOut?.Invoke();
                _invokedTimedOut = true;
            }
            else if (_elapsedTime.TotalSeconds >= 1 && !_invokedTimedOut)
            {
                NumericTime = TurnMaxDuration - (float)_elapsedTime.TotalSeconds;
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

