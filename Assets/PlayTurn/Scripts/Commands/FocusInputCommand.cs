﻿using TopicTwister.Shared.Interfaces;
using TopicTwister.PlayTurn.Keyboard;
using TMPro;
using UnityEngine;


namespace TopicTwister.PlayTurn.Commands
{
    public class FocusInputCommand : MonoBehaviour , ICommand
    {
        [SerializeField] private KeyboardController _keyboardController;
        private TextMeshProUGUI _userIput;

        private const string UserInputTag = "UserInput";

        public void Execute()
        {
            if (_userIput == null)
            {
                _userIput = gameObject.transform.Find(UserInputTag).GetComponent<TextMeshProUGUI>();
            }
            _keyboardController.TextField = _userIput;
        }
    }
}