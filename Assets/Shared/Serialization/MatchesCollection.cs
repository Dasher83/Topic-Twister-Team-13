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

        public MatchesCollection(MatchDTO[] matches)
        {
            _matches = matches;
        }

        public List<MatchDTO> Matches
        {
            get
            {
                if( _matches == null )
                {
                    return new List<MatchDTO>();
                }
                return _matches.ToList();
            }
        }
    }
}
