using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class UserMatchNotSabedByRepositoryException : Exception
    {
        public UserMatchNotSabedByRepositoryException() { }
        public UserMatchNotSabedByRepositoryException(string message) : base(message) { }
    }
}
