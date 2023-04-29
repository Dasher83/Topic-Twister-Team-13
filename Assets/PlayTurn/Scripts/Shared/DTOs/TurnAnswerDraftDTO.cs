using System;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.PlayTurn.Shared.DTOs
{
    [Serializable]
    public class TurnAnswerDraftDTO
    {
        [SerializeField] private CategoryDto _category;
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;

        private TurnAnswerDraftDTO() { }

        public TurnAnswerDraftDTO(CategoryDto category, int order)
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
