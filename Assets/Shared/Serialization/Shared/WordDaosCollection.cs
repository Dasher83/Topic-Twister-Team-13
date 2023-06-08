using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class WordDaosCollection
    {
        [SerializeField] private WordDaoJson[] _words;

        public WordDaosCollection(WordDaoJson[] words)
        {
            _words = words;
        }

        public List<WordDaoJson> Words
        {
            get
            {
                if (_words == null)
                {
                    return new List<WordDaoJson>();
                }
                return _words.ToList();
            }
        }
    }
}
