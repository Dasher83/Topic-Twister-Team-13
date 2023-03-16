using TMPro;
using UnityEngine;


namespace TopicTwister.PlayRound.Scripts.Keyboard
{
    public class KeyboardController : MonoBehaviour
    {
        private TextMeshProUGUI _textField;

        private string _currentInput;

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
        }

        public void AddLetter(string letter)
        {
            _currentInput += letter; 
            _textField.text = _currentInput;
        }

        public void EreaseLetter()
        {
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            _textField.text = _currentInput;
        }

        public void AddSpace()
        {
            _currentInput += " ";
        }
    }
}
