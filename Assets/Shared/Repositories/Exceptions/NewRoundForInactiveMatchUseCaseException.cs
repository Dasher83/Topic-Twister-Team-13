using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class NewRoundForInactiveMatchUseCaseException : Exception
    {
        public NewRoundForInactiveMatchUseCaseException(string message) : base(message) { }
    }
}
