using UnityEngine;
using System;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class AnswerDto
    {
        [SerializeField] private CategoryDto _categoryDto;
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;

        private AnswerDto() { }

        public AnswerDto(CategoryDto categoryDto, string userInput, int order)
        {
            _categoryDto = categoryDto;
            _userInput = userInput;
            _order = order;
        }

        public CategoryDto Category => _categoryDto;
        public string UserInput => _userInput;
        public int Order => _order;
    }
}
