using System;


namespace TopicTwister.Shared.DTOs
{
    public class UserRoundDto
    {
        private int _userId;
        private int _roundId;
        private bool _isWinner;
        private int _points;

        public int UserId => _userId;
        public int RoundId => _roundId;
        public bool IsWinner => _isWinner;
        public int Points => _points;

        public UserRoundDto(int userId, int roundId, bool isWinner, int points)
        {
            _userId = userId;
            _roundId = roundId;
            _isWinner = isWinner;
            _points = points;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            UserRoundDto other = (UserRoundDto)obj;

            bool userIdEquals = other._userId == _userId;
            bool roundIdEquals = other._roundId == _roundId;
            bool isWinnerEquals = other._isWinner == _isWinner;
            bool isPointsEquals = other._points == _points;

            return userIdEquals && roundIdEquals && isWinnerEquals && isPointsEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_userId, _roundId, _isWinner, _points);
        }
    }
}
