namespace TopicTwister.Shared.Models
{
    public class User
    {
        private readonly int _id;

        public int Id => _id;

        public User(int id)
        {
            _id = id;
        }
    }
}