using System;


namespace TopicTwister.Shared.Models
{
    public class UserRound
    {
        private User _user;
        private Round _round;
        private bool _isWinner;
        private int _points;

        public User User => _user;
        public Round Round => _round;
        public bool IsWinner => _isWinner;
        public int Points => _points;

        public UserRound(User user, Round round, bool isWinner, int points)
        {
            _user = user;
            _round = round;
            _isWinner = isWinner;
            _points = points;
        }

        public override bool Equals(object obj)
        {
            if (obj == null && GetType() != obj.GetType())
                return false;

            UserRound other = (UserRound)obj;

            return _user.Equals(other._user) &&
                _round.Equals(other._round) &&
                _isWinner == other._isWinner &&
                _points == other._points;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_user, _round, _isWinner, _points);
        }
    }
}
