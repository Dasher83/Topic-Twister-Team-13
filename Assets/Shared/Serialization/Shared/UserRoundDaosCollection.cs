using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class UserRoundDaosCollection
    {
        [SerializeField] private UserRoundDaoJson[] _userRounds;

        public UserRoundDaosCollection(UserRoundDaoJson[] userRounds)
        {
            _userRounds = userRounds;
        }

        public List<UserRoundDaoJson> UserRounds
        {
            get
            {
                if (_userRounds == null)
                {
                    return new List<UserRoundDaoJson>();
                }
                return _userRounds.ToList();
            }
        }
    }
}
