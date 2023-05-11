using System;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.DTOs
{
    public class TurnDto
    {
        private int _userId;
        private int _roundId;
        private int _points;
        private string _startDateTime;
        private string _endDateTime;

        public int UserId => _userId;
        public int RoundId => _roundId;
        public int Points => _points;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);

        private TurnDto() { }

        public TurnDto(int userId, int roundId, int points, DateTime startDateTime, DateTime? endDateTime = null)
        {
            _userId = userId;
            _roundId = roundId;
            _points = points;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            TurnDto other = (TurnDto)obj;

            if (string.IsNullOrEmpty(other._endDateTime) && !string.IsNullOrEmpty(this._endDateTime)) return false;
            if (!string.IsNullOrEmpty(other._endDateTime) && string.IsNullOrEmpty(this._endDateTime)) return false;

            TimeSpan startDateTimesDifference = other.StartDateTime - this.StartDateTime;

            bool userIdEquals = _userId == other._userId;
            bool roundIdEquals = _roundId == other._roundId;
            bool pointsEquals = _points == other._points;
            bool startDateTimeAlmostEquals = startDateTimesDifference.TotalSeconds < 1;
            bool endDateTimeAlmostEquals =
                (string.IsNullOrEmpty(this._endDateTime) ||
                ((DateTime)other.EndDateTime - (DateTime)this.EndDateTime).TotalSeconds < 1);

            return userIdEquals && roundIdEquals && pointsEquals && startDateTimeAlmostEquals && endDateTimeAlmostEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_userId, _roundId, _startDateTime);
        }
    }
}
