using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class TurnDaosCollection
    {
        [SerializeField] private TurnDaoJson[] _turns;

        public TurnDaosCollection(TurnDaoJson[] turns)
        {
            _turns = turns;
        }

        public List<TurnDaoJson> Turns
        {
            get
            {
                if (_turns == null)
                {
                    return new List<TurnDaoJson>();
                }

                return _turns.ToList();
            }
        }
    }
}
