using System.Collections.Generic;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewRoundData", menuName = "ScriptableObjects/NewRoundData")]
    public class NewRoundScriptable : ScriptableObject
    {
        [SerializeField]
        private List<string> _categories = new List<string>();
        [SerializeField]
        private char _initialLetter;
        [SerializeField]
        private int _roundNumber;

        public void Initialize(List<string> categories, char initialLetter, int roundNumber)
        {
            _categories = categories;
            _initialLetter = initialLetter;
            _roundNumber = roundNumber;
        }
    }
}
