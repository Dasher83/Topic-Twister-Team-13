using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class RoundNotCreatedInUseCaseException : Exception
    {
        public RoundNotCreatedInUseCaseException() { }
        public RoundNotCreatedInUseCaseException(string message) : base(message) { }
        public RoundNotCreatedInUseCaseException(string message = "", Exception inner = null) : base(message, inner) { }
    }
}
