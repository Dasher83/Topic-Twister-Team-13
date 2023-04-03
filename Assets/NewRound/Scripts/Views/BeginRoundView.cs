using System;
using System.Collections.Generic;
using TMPro;
using TopicTwister.NewRound.Shared.ScriptableObjects;
using TopicTwister.Shared.ScriptableObjects;
using UnityEngine;


namespace TopicTwister.NewRound.Views
{
    public class BeginRoundView : MonoBehaviour
    {
        [SerializeField]
        private NewRoundScriptable _newRoundData;
        [SerializeField]
        private InitialLetterRevealedEventScriptable _eventContainer;
        [SerializeField]
        private TextMeshProUGUI _roundNumber;
        [SerializeField]
        private Transform _categoryListRoot;
        [SerializeField]
        private TextMeshProUGUI _initialLetter;

        private void Start()
        {
            _eventContainer.InitialLetterRevealed += InitialLetterRevealedEventHandler;
        }

        private void InitialLetterRevealedEventHandler()
        {
            char initialLetter = _initialLetter.text[0];
            int roundNumber = Int32.Parse(_roundNumber.text.Split(" ")[1]);
            List<string> categories = new List<string>();
            string categoryName;

            foreach (Transform childTransform in _categoryListRoot.transform)
            {
                categoryName = childTransform.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
                categories.Add(categoryName);
            }

            _newRoundData.Initialize(categories, initialLetter, roundNumber);
        }
    }
}
