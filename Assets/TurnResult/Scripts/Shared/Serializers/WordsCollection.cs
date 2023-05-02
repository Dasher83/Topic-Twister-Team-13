using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.TurnResult.Shared.Serializers
{
    [Serializable]
    public class WordsCollection
    {
        [SerializeField] private WordDto[] _words;

        public List<WordDto> Words => _words.ToList();
    }
}
