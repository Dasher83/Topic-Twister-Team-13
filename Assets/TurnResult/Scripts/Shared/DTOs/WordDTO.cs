using System;


namespace TopicTwister.TurnResult.Shared.DTOs
{
    [Serializable]
    public class WordDTO
    {
        private string _text;
        private char _initialLetter;
        private string _categoryId;

        private WordDTO() { }

        public WordDTO(string text, char initialLetter, string categoryId)
        {
            _text = text;
            _initialLetter = initialLetter;
            _categoryId = categoryId;
        }

        public string Text => _text;
        public char InitialLetter => _initialLetter;
        public string CategoryId => _categoryId;
    }
}
