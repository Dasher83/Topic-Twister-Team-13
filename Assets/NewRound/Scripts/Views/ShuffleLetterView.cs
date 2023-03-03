using TopicTwister.NewRound.Presenters;
using TopicTwister.NewRound.Shared.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace TopicTwister.NewRound.Views
{
    public class ShuffleLetterView : MonoBehaviour, IShuffleLetterView
    {
        private IShuffleLetterPresenter _shuffleLetterPresenter;
        private TextMeshProUGUI _textComponent;
        private Button _button;
        
        void Start()
        {
            _button = GetComponentInChildren<Button>();
            _textComponent = GetComponentInChildren<TextMeshProUGUI>();
            _shuffleLetterPresenter = new ShuffleLetterPresenter(this);
        }

        public void GetRandomLetter()
        {
            _shuffleLetterPresenter.GetRandomLetter();
        }

        public void ShowLetter(string letter)
        {
            _textComponent.text = letter;
            _button.enabled = false; 
        }
    }
}