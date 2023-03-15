using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Debug.Log(_currentInput);
        }


        public void AddLetter(string letter)
        {
            _currentInput += letter; 
            textField.text = _currentInput; 
            Debug.Log(_currentInput);
        }

        public void EreaseLetter()
        {
            _currentInput = _currentInput[..^1]; // <- Borra la ultima letra del string :D
            textField.text = _currentInput;
        }

        public void AddSpace()
        {
            _currentInput += " ";
        }
    }
}
