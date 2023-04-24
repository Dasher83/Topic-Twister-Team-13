using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class UserNotFoundByRepositoryException : Exception
    {
        public UserNotFoundByRepositoryException() { }

        public UserNotFoundByRepositoryException(string message) : base(message) { }
    } 
}
