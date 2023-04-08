using UnityEngine;
using System;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public struct TurnAnswerDTO
    {
        [SerializeField] private CategoryDTO _category;
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;

        public TurnAnswerDTO(CategoryDTO category, string userInput, int order)
        {
            _category = category;
            _userInput = userInput;
            _order = order;
        }

        public CategoryDTO Category => _category;
        public string UserInput => _userInput;
        public int Order => _order;
    }
}
