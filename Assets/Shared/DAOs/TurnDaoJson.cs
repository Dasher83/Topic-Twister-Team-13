using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class TurnDaoJson
    {
        [SerializeField] private int _userId;
        [SerializeField] private int _roundId;
        [SerializeField] private string _startDateTime;

        public int UserId => _userId;
        public int RoundId => _roundId;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);

        private TurnDaoJson() { }

        public TurnDaoJson(int userId, int roundId, string startDateTime)
        {
            _userId = userId;
            _roundId = roundId;
            _startDateTime = startDateTime;
        }
    } 
}
