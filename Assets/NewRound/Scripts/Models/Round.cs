namespace TopicTwister.NewRound.Models
{
    public class Round
    {
        private int _id;

        public int Id => _id;

        public Round() { }

        public Round(int id) 
        {
            _id = id;
        }
    }
}
