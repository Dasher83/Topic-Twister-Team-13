using System;


namespace TopicTwister.Shared.Models
{
    public class Turn
    {
        private User _user;
        private Round _round;
        private string _startDateTime;
        private string _endDateTime;

        public User User => _user;
        public Round Round => _round;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);
        public DateTime? EndDateTime => string.IsNullOrEmpty(_endDateTime) ? null : DateTime.Parse(_endDateTime);

        private Turn(){}
        
        public Turn(User user, Round round, DateTime startDateTime, DateTime? endDateTime = null)
        {
            _user = user;
            _round = round;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
            _endDateTime = endDateTime == null ? "" : ((DateTime)endDateTime).ToString("s"); //ISO 8601
        }
    }
}
