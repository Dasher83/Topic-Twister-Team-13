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

        public CategoryDto CategoryDto => _categoryDto;
        public string UserInput => _userInput;
        public int Order => _order;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            AnswerDto other = (AnswerDto)obj;

            bool categoryDtoEquals = _categoryDto.Equals(other.CategoryDto);
            bool userInputEquals = _userInput == other._userInput;
            bool orderEquals = _order == other._order;

            return categoryDtoEquals && userInputEquals && orderEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_categoryDto, _userInput, _order);
        }
    }
}
