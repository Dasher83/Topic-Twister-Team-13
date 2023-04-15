using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class MatchDTO
    {
        [SerializeField] private int _id;
        [SerializeField] private DateTime _startDateTime;
        [SerializeField] private DateTime? _endDateTime;

        public int Id => _id;
        public DateTime StartDate => _startDateTime;
        public DateTime? EndDate => _endDateTime;

        public MatchDTO(int id, DateTime startDateTime, DateTime? endDateTime = null)
        {
            _id = id;
            _startDateTime = startDateTime;
            _endDateTime = endDateTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            MatchDTO other = (MatchDTO)obj;

            if (other._endDateTime == null && this._endDateTime != null) return false;
            if (other._endDateTime != null && this._endDateTime == null) return false;

            TimeSpan startDifference = other._startDateTime - this._startDateTime;

            return this._id == other._id &&
                startDifference.TotalSeconds < 1 &&
                (this._endDateTime == null || ((DateTime)other._endDateTime - (DateTime)this._endDateTime).TotalSeconds < 1);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _startDateTime, _endDateTime);
        }
    }
}
