namespace TopicTwister.Shared.Models
{
    public class Turn
    {
        private User _user;
        private Round _round;

        private Turn(){}
        
        public Turn(User user, Round round)
        {
            _user = user;
            _round = round;
        }
    }
}
