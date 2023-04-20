using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class UserMatchNotPersistedByRepositoryException : Exception
    {
        public UserMatchNotPersistedByRepositoryException() { }
        public UserMatchNotPersistedByRepositoryException(string message) : base(message) { }
    }
}
