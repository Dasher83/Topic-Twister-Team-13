using System.Collections.Generic;
using System.Linq;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Serializers;
using UnityEngine;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;


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

        public Result<bool> Exists(string text, int categoryId, char initialLetter)
        {
            bool exists = _words
                .Any(
                    word => word.Text.ToLower() == text.ToLower() &&
                    word.CategoryId == categoryId &&
                    word.Text.ToLower()[0] == initialLetter.ToString().ToLower()[0]);

            Result<bool> filterWordsOperationResult = Result<bool>.Success(outcome: exists);
            return filterWordsOperationResult;
        }
    }
}
