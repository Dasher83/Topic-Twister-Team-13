using TMPro;
using UnityEngine;


namespace Assets.PlayRound.Scripts.Keyboard
{
    public class KeyboardController : MonoBehaviour
    {
        public TextMeshProUGUI textField; 

        private string _currentInput = "";

        public void Start()
        {
            _currentInput = textField.text;
        }


        public void AddLetter(string letter)
        {
            _currentInput += letter; 
            textField.text = _currentInput;
        }

        public void EreaseLetter()
        {
            _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
            textField.text = _currentInput;
        }

        public void AddSpace()
        {
            _currentInput += " ";
        }
    }
}
