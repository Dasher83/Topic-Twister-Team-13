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

        public Match(int id, DateTime startDateTime, DateTime? endDateTime = null)
        {
            _id = id;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
        }

        public bool IsActive => string.IsNullOrEmpty(_endDateTime);

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(_startDateTime)) return false;
                if (string.IsNullOrEmpty(_endDateTime) == false && StartDateTime > EndDateTime) return false;
                return true;
            }
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
