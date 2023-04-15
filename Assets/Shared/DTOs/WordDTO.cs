using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class WordDTO
    {
        [SerializeField] private string _text;
        [SerializeField] private string _categoryId;

        private WordDTO() { }

        public WordDTO(string text, string categoryId)
        {
            _text = text;
            _categoryId = categoryId;
        }

        public string Text => _text;
        public string CategoryId => _categoryId;
    }
}
