using System;


namespace TopicTwister.Shared.Models
{
    public class Turn
    {
        private User _user;
        private Round _round;
        private string _startDateTime;

        public User User => _user;
        public Round Round => _round;
        public DateTime StartDateTime => DateTime.Parse(_startDateTime);

        private Turn(){}
        
        public Turn(User user, Round round, DateTime startDateTime)
        {
            _user = user;
            _round = round;
            _startDateTime = startDateTime.ToString("s"); //ISO 8601
        }
    }
}
