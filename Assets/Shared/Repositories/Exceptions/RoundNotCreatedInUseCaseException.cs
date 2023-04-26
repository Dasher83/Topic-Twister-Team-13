using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class RoundNotCreatedInUseCaseException : Exception
    {
        public RoundNotCreatedInUseCaseException() { }
        public RoundNotCreatedInUseCaseException(string message) : base(message) { }
        public RoundNotCreatedInUseCaseException(string mensaje = "", Exception inner = null) : base(mensaje, inner) { }
    }
}
