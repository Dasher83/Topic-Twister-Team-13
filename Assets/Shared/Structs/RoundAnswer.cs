using UnityEngine;
using System;

namespace TopicTwister.Shared.Structs
{
    [Serializable]
    public struct RoundAnswer
    {
        [SerializeField] private string _categoryId;
        [SerializeField] private string _userInput;

        public RoundAnswer(string categoryId, string userInput)
        {
            _categoryId = categoryId;
            _userInput = userInput;
        }

        public string CategoryId =>  _categoryId;
        public string UserInput => _userInput;
    }
}
