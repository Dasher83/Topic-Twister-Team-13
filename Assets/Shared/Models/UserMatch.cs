using System;


namespace TopicTwister.Shared.Models
{
    public class UserMatch
    {
        private int _score;
        private bool _isWinner;
        private bool _hasInitiative;
        private User _user;
        private Match _match;

        public int Score => _score;
        public bool IsWinner => _isWinner;
        public bool HasInitiative => _hasInitiative;
        public User User => _user;
        public Match Match => _match;

        public UserMatch(int score, bool isWinner, bool hasInitiative, User user, Match match)
        {
            _score = score;
            _isWinner = isWinner;
            _hasInitiative = hasInitiative;
            _user = user;
            _match = match;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            UserMatch other = (UserMatch)obj;

            return _user.Equals(other._user) &&
                _match.Equals(other._match) &&
                _score == other._score &&
                _isWinner == other._isWinner &&
                _hasInitiative == other._hasInitiative;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_user, _match, _score, _isWinner, _hasInitiative);
        }
    }
}
