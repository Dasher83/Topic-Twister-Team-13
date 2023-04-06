using UnityEngine;
using System;

namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public struct RoundAnswerDTO
    {
        [SerializeField] private string _categoryId;
        [SerializeField] private string _userInput;
        [SerializeField] private int _order;

        public RoundAnswerDTO(string categoryId, string userInput, int order)
        {
            _categoryId = categoryId;
            _userInput = userInput;
            _order = order;
        }

        public string CategoryId => _categoryId;
        public string UserInput => _userInput;
        public int Order => _order;
    }
}
