using System.Collections.Generic;
using System.Linq;
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

        public List<string> Categories => _categories.ToList();

        public char InitialLetter => _initialLetter;

        public int RoundNumber => _roundNumber;

        public void Initialize(List<string> categories, char initialLetter, int roundNumber)
        {
            _categories = categories;
            _initialLetter = initialLetter;
            _roundNumber = roundNumber;
        }
    }
}
