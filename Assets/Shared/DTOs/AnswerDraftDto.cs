using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class AnswerDraftDto
    {
        [SerializeField] private CategoryDto _category;
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;

        private AnswerDraftDto() { }

        public AnswerDraftDto(CategoryDto category, int order)
        {
            _category = category;
            _userInput = "";
            _order = order;
        }

        public CategoryDto Category => _category;
        
        public string UserInput 
        {
            get 
            {
                return _userInput;
            }

            set
            {
                _userInput = value;
            }
        }
        
        public int Order => _order;
    }
}
