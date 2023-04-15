using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization
{
    [Serializable]
    public class MatchesCollection
    {
        [SerializeField] private MatchDTO[] _matches;

        public List<MatchDTO> Matches => _matches.ToList();
    }
}
