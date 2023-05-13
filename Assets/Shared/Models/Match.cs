using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Models
{
    public class Match
    {
        private int _id;
        private string _startDateTime;
        private string _endDateTime;
        private List<Round> _rounds;
        private UserMatch[] _userMatches;

        public int Id => _id;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);
        public List<Round> Rounds => _rounds.ToList();
        public UserMatch[] UserMatch => _userMatches.ToArray();

        public Match()
        {
            _id = -1; // No id assigend yet
            _startDateTime = DateTime.UtcNow.ToString("s"); //ISO 8601
            _endDateTime = "";
            _rounds = null;
            _userMatches = null;
        }

        public Match(DateTime startDateTime, DateTime? endDateTime = null)
        {
            _id = -1;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
            _rounds = null;
            _userMatches = null;
        }

        public Match(
            int id, DateTime startDateTime, DateTime? endDateTime = null,
            List<Round> rounds = null, UserMatch[] userMatches = null)
        {
            _id = id;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
            _rounds = rounds;
            _userMatches = userMatches;
        }

        public bool IsActive => string.IsNullOrEmpty(_endDateTime);

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(_startDateTime)) return false;
                if (string.IsNullOrEmpty(_endDateTime) == false && StartDateTime > EndDateTime) return false;
                if (_rounds.Count > 3) return false;
                return true;
            }
        }
        public bool AreAllRoundsCreated => _rounds.Count == 3;

        public Round? ActiveRound
        {
            get
            {
                if (_rounds.Count == 0) return null;

                return _rounds.SingleOrDefault(round => round.IsActive);
            }
        }

        public Operation<bool> UserIsInMatch(int userId)
        {
            if (_userMatches == null || (_userMatches[0].User.Id != userId && _userMatches[1].User.Id != userId))
            {
                return Operation<bool>.Failure(
                    errorMessage: $"User with id {userId} is not involved in match with id {_id}");
            }

            return Operation<bool>.Success(result: true);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Match other = (Match)obj;

            if (string.IsNullOrEmpty(other._endDateTime) && !string.IsNullOrEmpty(this._endDateTime)) return false;
            if (!string.IsNullOrEmpty(other._endDateTime) && string.IsNullOrEmpty(this._endDateTime)) return false;

            TimeSpan startDifference = other.StartDateTime - this.StartDateTime;

            bool areIdsEquals = this._id == other._id;
            bool isStarDifferenceCloseEnough = startDifference.TotalSeconds < 1;
            bool isEndDateTimeCloseEnough = string.IsNullOrEmpty(this._endDateTime);
            if (!isEndDateTimeCloseEnough)
            {
                isEndDateTimeCloseEnough = ((DateTime)other.EndDateTime - (DateTime)this.EndDateTime).TotalSeconds < 1;
            }

            return areIdsEquals && isStarDifferenceCloseEnough && isEndDateTimeCloseEnough;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _startDateTime, _endDateTime);
        }
    }
}
