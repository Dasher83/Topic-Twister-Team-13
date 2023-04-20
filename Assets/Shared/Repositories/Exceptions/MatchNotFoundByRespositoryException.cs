using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class MatchNotFoundByRespositoryException : Exception
    {
        public MatchNotFoundByRespositoryException() { }

        public MatchNotFoundByRespositoryException(string message) : base(message) { }
    }
}
