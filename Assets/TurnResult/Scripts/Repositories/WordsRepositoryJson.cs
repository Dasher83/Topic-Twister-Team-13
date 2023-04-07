using System.Collections.Generic;
using System.Linq;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Serializers;
using UnityEngine;


namespace TopicTwister.TurnResult.Repositories
{
    public class WordsRepositoryJson: IWordsRepository
    {
        private readonly List<WordDTO> _words;

        public WordsRepositoryJson(string wordsResourceName)
        {
            string data = Resources.Load<TextAsset>(wordsResourceName).text;

            _words = JsonUtility.FromJson<WordsCollection>(data).Words;
        }

        public bool Exists(string text, string categoryId, char initialLetter)
        {
            int a = 0;
            return _words.Any(
                word => word.Text == text &&
                word.CategoryId == categoryId &&
                word.InitialLetter == initialLetter);
        }
    }
}
