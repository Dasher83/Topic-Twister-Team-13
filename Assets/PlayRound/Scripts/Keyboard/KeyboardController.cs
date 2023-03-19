using TMPro;
using UnityEngine;

namespace TopicTwister.PlayRound.Scripts.Keyboard
{
    public class KeyboardController : MonoBehaviour
    {
        private TextMeshProUGUI _textField;
        [SerializeField] private Timer.Timer currentTimer;
        [SerializeField] private StopButton.StopButton stopButton;

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
            currentTimer.timedOut.AddListener(InputEndEventHandler);
            stopButton.InterruptRound.AddListener(InputEndEventHandler);
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

        private void InputEndEventHandler()
        {
            _blockKeyboard = true;
        }
    }
}
