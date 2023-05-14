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

        public List<WordDaoJson> Words => _words.ToList();
    }
}
