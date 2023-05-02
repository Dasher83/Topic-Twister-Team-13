using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class WordDto
    {
        [SerializeField] private string _text;
        [SerializeField] private int _categoryId;

        private WordDto() { }

        public WordDto(string text, int categoryId)
        {
            _text = text;
            _categoryId = categoryId;
        }

        public string Text => _text;
        public int CategoryId => _categoryId;
    }
}
