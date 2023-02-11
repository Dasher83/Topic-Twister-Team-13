using UnityEngine;
using TMPro;

namespace TopicTwister.Scripts.UI
{
    public class Shuffle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textComponent;
        
        public void ShuffleLetters()
        {
            textComponent.text = GetRandomLetter();
        }

        private string GetRandomLetter()
        {
            int number = Random.Range(0, 26);
            char letter = (char)(((int)'A') + number);
            return $"{ letter }";
        }
    }
}
