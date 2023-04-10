using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewRoundData", menuName = "ScriptableObjects/NewRoundData")]
    public class NewRoundScriptable : ScriptableObject
    {
        [SerializeField]
        private List<CategoryDTO> _categories;
        [SerializeField]
        private char _initialLetter;
        [SerializeField]
        private int _roundNumber;
        [SerializeField]
        private RoundEndedEventScriptable _eventContainer;

        public List<CategoryDTO> Categories => _categories.ToList();

        public char InitialLetter => _initialLetter;

        public int RoundNumber => _roundNumber;

        public void Initialize()
        {
            _roundNumber = 1;
            _eventContainer.RoundEnded += GoToNextRound;
        }

        public void Initialize(char initialLetter)
        {
            _initialLetter = initialLetter;
        }

        public void Initialize(CategoryDTO[] categories)
        {
            _categories = new List<CategoryDTO>();
            _categories = categories.ToList();
        }

        private void GoToNextRound()
        {
            _roundNumber++;
        }
    }
}
