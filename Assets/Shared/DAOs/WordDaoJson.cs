using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class WordDaoJson
    {
        [SerializeField] private string _text;
        [SerializeField] private int _categoryId;

        private WordDaoJson() { }

        public WordDaoJson(string text, int categoryId)
        {
            _text = text;
            _categoryId = categoryId;
        }

        public string Text => _text;
        public int CategoryId => _categoryId;
    }
}
