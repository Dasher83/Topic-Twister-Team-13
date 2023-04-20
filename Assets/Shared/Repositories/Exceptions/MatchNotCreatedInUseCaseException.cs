using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class MatchNotCreatedInUseCaseException : Exception
    {
        public MatchNotCreatedInUseCaseException() { }
        public MatchNotCreatedInUseCaseException(string message) : base(message) { }
        public MatchNotCreatedInUseCaseException(string mensaje = "", Exception inner = null) : base(mensaje, inner) { }
    }
}
