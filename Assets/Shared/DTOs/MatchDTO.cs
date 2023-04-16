﻿using System;
using UnityEngine;


namespace TopicTwister.Shared.DTOs
{
    [Serializable]
    public class MatchDTO
    {
        [SerializeField] private int _id;
        [SerializeField] private string _startDateTime;
        [SerializeField] private string _endDateTime;

        public int Id => _id;

        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);

        public MatchDTO(int id, DateTime startDateTime, DateTime? endDateTime = null)
        {
            _id = id;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? null : ((DateTime)endDateTime).ToString("s");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            MatchDTO other = (MatchDTO)obj;

            if (string.IsNullOrEmpty(other._endDateTime) && !string.IsNullOrEmpty(this._endDateTime)) return false;
            if (!string.IsNullOrEmpty(other._endDateTime) && string.IsNullOrEmpty(this._endDateTime)) return false;

            TimeSpan startDifference = other.StartDateTime - this.StartDateTime;

            return this._id == other._id &&
                startDifference.TotalSeconds < 1 &&
                (this._endDateTime == null || ((DateTime)other.EndDateTime - (DateTime)this.EndDateTime).TotalSeconds < 1);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _startDateTime, _endDateTime);
        }
    }
}
