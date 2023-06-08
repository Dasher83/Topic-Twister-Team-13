using System;
using UnityEngine;


namespace TopicTwister.Shared.DAOs
{
    [Serializable]
    public class UserRoundDaoJson
    {
        [SerializeField] private int _userId;
        [SerializeField] private int _roundId;
        [SerializeField] private bool _isWinner;
        [SerializeField] private int _points;

        public int UserId => _userId;
        public int RoundId => _roundId;
        public bool IsWinner => _isWinner;
        public int Points => _points;

        public UserRoundDaoJson(int userId, int roundId, bool isWinner, int points)
        {
            _userId = userId;
            _roundId = roundId;
            _isWinner = isWinner;
            _points = points;
        }

        public override bool Equals(object obj)
        {
            if (obj == null && GetType() != obj.GetType())
                return false;

            UserRoundDaoJson other = (UserRoundDaoJson)obj;

            return _userId == other._userId &&
                _roundId == other._roundId &&
                _isWinner == other._isWinner &&
                _points == other._points;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_userId, _roundId, _isWinner, _points);
        }
    }
}
