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
        [SerializeField] private bool _isCorrect;

        private AnswerDto() { }

        public AnswerDto(CategoryDto categoryDto, string userInput, int order, bool isCorrect)
        {
            _categoryDto = categoryDto;
            _userInput = userInput;
            _order = order;
            _isCorrect = isCorrect;
        }

        public CategoryDto CategoryDto => _categoryDto;
        public string UserInput => _userInput;
        public int Order => _order;
        public bool IsCorrect => _isCorrect;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            AnswerDto other = (AnswerDto)obj;

            bool categoryDtoEquals = _categoryDto.Equals(other.CategoryDto);
            bool userInputEquals = _userInput == other._userInput;
            bool orderEquals = _order == other._order;
            bool isCorrectEquals = _isCorrect == other._isCorrect;

            return categoryDtoEquals && userInputEquals && orderEquals && isCorrectEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_categoryDto, _userInput, _order, _isCorrect);
        }
    }
}
