using System;


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


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            User other = (User)obj;

            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id);
        }
    }
}
