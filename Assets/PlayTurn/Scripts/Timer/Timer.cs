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
        private DateTime _timeInit;
        private TimeSpan _timerTime;
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;

        void Start()
        {
            _timerText = GetComponentInChildren<TextMeshProUGUI>();
            _numericTime = float.Parse(_timerText.text);
            _invokedTimedOut = false;
            _interruptTurnEventContainer.InterruptTurn += InterruptTurnEventHandler;
            _timeInit = DateTime.UtcNow;
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
            if (DifferenceTimes())
            {
                
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
        
        private bool DifferenceTimes()
        {
            
            
            _timerTime = DateTime.UtcNow.Subtract(_timeInit);

            
            if (_timerTime.TotalSeconds > 60)
            {
                NumericTime = 0f;
                return false;
            }

            if (_timerTime.TotalSeconds >= 1)
            {
                NumericTime = 60f - (float)_timerTime.TotalSeconds;
                return true;
            }

            if (_timerTime.TotalSeconds > 60)
            {
                return false;
            }
            return true;
        }

        private void InterruptTurnEventHandler()
        {
            NumericTime = 0;
            _invokedTimedOut = true;
        }

        private void UpdateTimerText() => _timerText.text = string.Format("{0:0}", _numericTime);
    }
}

