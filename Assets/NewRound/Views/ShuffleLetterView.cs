using System.Collections;
using System.Collections.Generic;
using TMPro;
using TopicTwister.NewRound.Shared.Interfaces;
using UnityEngine;
using UnityEngine.UI;


namespace TopicTwister.NewRound.Views
{
    public class ShuffleLetterView : MonoBehaviour, IShuffleLetterView
    {
        private IShuffleLetterPresenter _shuffleLetterPresenter;
        private TextMeshProUGUI _textComponent;
        private Button _button;
        
        void Start()
        {
            _textComponent = GetComponentInChildren<TextMeshProUGUI>();;
            _button = GetComponentInChildren<Button>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShowLetter(string letter)
        {
            _textComponent.text = letter;
            _button.enabled = false; 
        }
    }
}