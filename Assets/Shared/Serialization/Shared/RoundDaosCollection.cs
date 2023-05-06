using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class RoundDaosCollection
    {
        [SerializeField] private RoundDaoJson[] _rounds;

        public RoundDaosCollection(RoundDaoJson[] roundDaos)
        {
            _rounds = roundDaos;
        }

        public List<RoundDaoJson> RoundDaos
        {
            get
            {
                if(_rounds == null )
                {
                    return new List<RoundDaoJson>();
                }
                return _rounds.ToList();
            }
        }
    }
}
