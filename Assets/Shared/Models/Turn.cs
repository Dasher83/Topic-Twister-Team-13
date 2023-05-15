using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;

namespace TopicTwister.Shared.Models
{
    public class Turn
    {
        private User _user;
        private Round _round;
        private int _points;
        private string _startDateTime;
        private string _endDateTime;

        public User User => _user;
        public Round Round => _round;
        public int Points => _points;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);

        private Turn(){}

        public Turn(
            User user, Round round, DateTime startDateTime, DateTime endDateTime, int points)
        {
            _user = user;
            _round = round;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime.ToString("s"); //ISO 8601
            _points = points;
        }

        public Turn(
            User user, Round round, DateTime startDateTime, DateTime? endDateTime = null,
            List<Answer> answers = null, IWordsRepository wordsRepository = null)
        {
            _user = user;
            _round = round;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
            _points = answers == null ? 0 : answers.Count(answer => answer.IsCorrect(wordsRepository));
        }

        public bool HasEnded => string.IsNullOrEmpty(_endDateTime) == false;

        public float DurationInSeconds
        {
            get
            {
                TimeSpan turnTimeSpan = (TimeSpan)(EndDateTime - StartDateTime);
                return (float)turnTimeSpan.TotalSeconds;
            }
        }
    }
}
