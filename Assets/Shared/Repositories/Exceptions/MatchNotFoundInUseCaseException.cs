using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class MatchNotFoundInUseCaseException : Exception
    {
        public MatchNotFoundInUseCaseException() { }
        public MatchNotFoundInUseCaseException(string message) : base(message) { }
    }
}
