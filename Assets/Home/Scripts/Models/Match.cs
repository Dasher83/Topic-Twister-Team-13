using System;


namespace TopicTwister.Home.Models
{
    public class Match
    {
        private readonly int _id;
        private readonly DateTime _startDateTime;
        private readonly DateTime? _endDateTime;

        public int Id => _id;
        public DateTime StartDate => _startDateTime;
        public DateTime? EndDate => _endDateTime;

        public Match(int id)
        {
            _id = id;
            _startDateTime = DateTime.UtcNow;
            _endDateTime = null;
        }
    }
}
