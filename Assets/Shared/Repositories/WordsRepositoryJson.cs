using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using UnityEngine;
using TopicTwister.Shared.Utils;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Deserializers;
using System.IO;


namespace TopicTwister.Shared.Repositories
{
    public class WordsRepositoryJson: IWordsRepository
    {
        private readonly string _path;
        private readonly List<WordDaoJson> _words;

        public WordsRepositoryJson(string resourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            string data = File.ReadAllText(_path);
            _words = new WordDaosCollectionDeserializer().Deserialize(data).Words;
        }

        public Operation<bool> Exists(string text, int categoryId, char initialLetter)
        {
            bool exists = _words
                .Any(
                    word => word.Text.ToLower() == text.ToLower() &&
                    word.CategoryId == categoryId &&
                    word.Text.ToLower()[0] == initialLetter.ToString().ToLower()[0]);

            Operation<bool> filterWordsOperationResult = Operation<bool>.Success(result: exists);
            return filterWordsOperationResult;
        }
    }
}
