using System.Collections.Generic;
using TMPro;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.PlayTurn.CategoryInputList
{
    public class UpdateCategoryInputList : MonoBehaviour
    {
        [SerializeField]
        private MatchCacheScriptable _matchCacheData;

        private List<TextMeshProUGUI> _texts;

        private void UpdateText(int index)
        {
            _texts[index].text = _matchCacheData.TurnAnswerDrafts[index].UserInput;
        }

        void Start()
        {
            _texts = new List<TextMeshProUGUI>();

            foreach (Transform child in transform)
            {
                _texts.Add(child.Find("UserInput").GetComponent<TextMeshProUGUI>());
            }
            _matchCacheData.UserInputChanged += UpdateText;
        }
    }
}
