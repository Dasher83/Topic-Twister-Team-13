using TMPro;
using UnityEngine;


namespace TopicTwister.PlayRound.Scripts.Keyboard
{
    public class KeyboardController : MonoBehaviour
    {
        private TextMeshProUGUI _textField;
        public Timer.Timer currentTimer;

        private string _currentInput;

        private bool _blockKeyboard;

        public TextMeshProUGUI TextField {
            set
            {
                _textField = value;
                _currentInput = _textField.text;
            }
        }

        public void Start()
        {
            _currentInput = "";
            _blockKeyboard = false;
            currentTimer.timedOut.AddListener(TimedOutEventHandler);
        }

        public void AddLetter(string letter)
        {
            if (_blockKeyboard) return;
            _currentInput += letter; 
            _textField.text = _currentInput;
        }

        public void EreaseLetter()
        {
            if (_blockKeyboard) return;
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            _textField.text = _currentInput;
        }

        public void AddSpace()
        {
            if (_blockKeyboard) return;
            _currentInput += " ";
        }

        private void TimedOutEventHandler()
        {
            _blockKeyboard = true;
        }
    }
}
