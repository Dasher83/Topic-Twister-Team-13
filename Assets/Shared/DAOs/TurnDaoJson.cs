using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class TurnDaoJson
    {
        [SerializeField] private int _userId;
        [SerializeField] private int _roundId;
        [SerializeField] private int _points;
        [SerializeField] private string _startDateTime;
        [SerializeField] private string _endDateTime;

        public int UserId => _userId;
        public int RoundId => _roundId;
        public int Points => _points;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);

        private TurnDaoJson() { }

        public TurnDaoJson(int userId, int roundId, int points, string startDateTime, string endDateTime)
        {
            _userId = userId;
            _roundId = roundId;
            _points = points;
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
        }
    } 
}
