using UnityEngine;
using System;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class TurnAnswerDTO
    {
        [SerializeField] private CategoryDto _category;
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;

        private TurnAnswerDTO() { }

        public TurnAnswerDTO(CategoryDto category, string userInput, int order)
        {
            _category = category;
            _userInput = userInput;
            _order = order;
        }

        public CategoryDto Category => _category;
        public string UserInput => _userInput;
        public int Order => _order;
    }
}
