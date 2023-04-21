using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class UserMatchDaosCollection
    {
        [SerializeField] private UserMatchDaoJson[] _userMatches;

        public UserMatchDaosCollection(UserMatchDaoJson[] userMatches)
        {
            _userMatches = userMatches;
        }

        public List<UserMatchDaoJson> UserMatches
        {
            get
            {
                if (_userMatches == null)
                {
                    return new List<UserMatchDaoJson>();
                }
                return _userMatches.ToList();
            }
        }
    }
}
