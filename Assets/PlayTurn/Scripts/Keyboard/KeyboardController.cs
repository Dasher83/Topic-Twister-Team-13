using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.PlayTurn.Keyboard
{
    public class KeyboardController : MonoBehaviour
    {
        private TextMeshProUGUI _textField;
        [SerializeField] private TimeOutEventScriptable _timeOutEventContainer;
        [SerializeField] private InterruptTurnEventScriptable _interruptTurnEventContainer;

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
            _timeOutEventContainer.TimeOut += InputEndEventHandler;
            _interruptTurnEventContainer.InterruptTurn += InputEndEventHandler;
        }

        public void AddLetter(string letter)
        {
            if (_blockKeyboard) return;
            if (_textField == null) return;
            _currentInput += letter; 
            _textField.text = _currentInput;
        }

        public void EreaseLetter()
        {
            if (_blockKeyboard) return;
            if (_textField == null) return;
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            _textField.text = _currentInput;
        }

        public void AddSpace()
        {
            if (_blockKeyboard) return;
            if (_textField == null) return;
            _currentInput += " ";
        }

        private void InputEndEventHandler()
        {
            _blockKeyboard = true;
        }
    }
}
