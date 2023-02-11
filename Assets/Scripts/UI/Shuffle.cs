using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace TopicTwister.Scripts.UI
{
    public class Shuffle : MonoBehaviour
    {
        private TextMeshProUGUI _textComponent;
        private Button _button;

        private void Start()
        {
            _textComponent = GetComponentInChildren<TextMeshProUGUI>();
            _button = GetComponentInChildren<Button>();
        }

        public void ShuffleLetters()
        {
            _textComponent.text = GetRandomLetter();
            _button.enabled = false;
        }

        private string GetRandomLetter()
        {
            int number = Random.Range(0, 26);
            char letter = (char)(((int)'A') + number);
            return $"{ letter }";
        }
    }
}
