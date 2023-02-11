using UnityEngine;
using TMPro;

namespace TopicTwister.Scripts.UI
{
    public class Shuffle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textComponent;
        
        public void ShuffleLetters()
        {
            textComponent.text = "A";
        }
    }
}
