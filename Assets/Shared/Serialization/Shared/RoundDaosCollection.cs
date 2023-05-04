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
        [SerializeField] private RoundDaoJson[] _roundDaos;

        public RoundDaosCollection(RoundDaoJson[] roundDaos)
        {
            _roundDaos = roundDaos;
        }

        public List<RoundDaoJson> RoundDaos
        {
            get
            {
                if(_roundDaos == null )
                {
                    return new List<RoundDaoJson>();
                }
                return _roundDaos.ToList();
            }
        }
    }
}
