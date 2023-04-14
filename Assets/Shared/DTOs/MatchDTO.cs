using System;


namespace TopicTwister.Shared.DTOs
{
    public class MatchDTO
    {
        private int _id;
        private DateTime _startDate;
        private DateTime _endDate;

        public int Id => _id;
        public DateTime StartDate => _startDate;
        public DateTime EndDate => _endDate;

        public MatchDTO(int id, DateTime startDate, DateTime endDate)
        {
            _id = id;
            _startDate = startDate;
            _endDate = endDate;
        }
    }
}
