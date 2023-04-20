using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class UserMatchNotFoundByRepositoryException : Exception
    {
        public UserMatchNotFoundByRepositoryException() { }
        public UserMatchNotFoundByRepositoryException(string message) : base(message) { }
    }
}
