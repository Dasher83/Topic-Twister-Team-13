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
        [SerializeField] private WordDTO[] _words;

        public List<WordDTO> Words => _words.ToList();
    }
}
