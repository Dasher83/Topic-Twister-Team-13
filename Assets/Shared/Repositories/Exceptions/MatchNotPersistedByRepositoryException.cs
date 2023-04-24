using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class MatchNotPersistedByRepositoryException : Exception
    {
        public MatchNotPersistedByRepositoryException() { }
        public MatchNotPersistedByRepositoryException(string message) : base(message) { }
    }
}
