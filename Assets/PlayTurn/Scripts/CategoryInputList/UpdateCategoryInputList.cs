using System.Collections.Generic;
using TMPro;
using TopicTwister.PlayTurn.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.PlayTurn.CategoryInputList
{
    public class UpdateCategoryInputList : MonoBehaviour
    {
        [SerializeField]
        private TurnAnswersDraftScriptable _turnAnswerDraftData;

        private List<TextMeshProUGUI> _texts;

        private void UpdateText(int index)
        {
            _texts[index].text = _turnAnswerDraftData.TurnAnswerDrafts[index].UserInput;
        }

        void Start()
        {
            _texts = new List<TextMeshProUGUI>();

            foreach (Transform child in transform)
            {
                _texts.Add(child.Find("UserInput").GetComponent<TextMeshProUGUI>());
            }
            _turnAnswerDraftData.UserInputChanged += UpdateText;
        }
    }
}
