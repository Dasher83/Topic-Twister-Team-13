using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class UserMatchesCollection
    {
        [SerializeField] private UserMatchDTO[] _userMatches;

        public UserMatchesCollection(UserMatchDTO[] userMatches)
        {
            _userMatches = userMatches;
        }

        public List<UserMatchDTO> UserMatches
        {
            get
            {
                if (_userMatches == null)
                {
                    return new List<UserMatchDTO>();
                }
                return _userMatches.ToList();
            }
        }
    }
}
