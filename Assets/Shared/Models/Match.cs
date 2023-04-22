using System;


namespace TopicTwister.Shared.Models
{
    public class Match
    {
        private int _id;
        private string _startDateTime;
        private string _endDateTime;

        public int Id => _id;

        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);

        public Match()
        {
            _id = -1; // No id assigend yet
            _startDateTime = DateTime.UtcNow.ToString("s"); //ISO 8601
            _endDateTime = "";
        }

        public Match(int id, DateTime startDateTime, DateTime? endDateTime)
        {
            _id = id;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Match other = (Match)obj;

            if (string.IsNullOrEmpty(other._endDateTime) && !string.IsNullOrEmpty(this._endDateTime)) return false;
            if (!string.IsNullOrEmpty(other._endDateTime) && string.IsNullOrEmpty(this._endDateTime)) return false;

            TimeSpan startDifference = other.StartDateTime - this.StartDateTime;

            return this._id == other._id &&
                startDifference.TotalSeconds < 1 &&
                (string.IsNullOrEmpty(this._endDateTime) || ((DateTime)other.EndDateTime - (DateTime)this.EndDateTime).TotalSeconds < 1);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _startDateTime, _endDateTime);
        }
    }
}