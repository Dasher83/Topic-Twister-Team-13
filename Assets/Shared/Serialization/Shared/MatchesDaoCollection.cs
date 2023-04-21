using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class MatchesDaoCollection
    {
        [SerializeField] private MatchDaoJson[] _matches;

        public MatchesDaoCollection(MatchDaoJson[] matches)
        {
            _matches = matches;
        }

        public List<MatchDaoJson> Matches
        {
            get
            {
                if( _matches == null )
                {
                    return new List<MatchDaoJson>();
                }
                return _matches.ToList();
            }
        }
    }
}
