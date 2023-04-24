using System;


namespace TopicTwister.Shared.Repositories.Exceptions
{
    public class UserNotFoundInUseCaseException : Exception
    {
        public UserNotFoundInUseCaseException() { }

        public UserNotFoundInUseCaseException(string message) : base(message) { }
    } 
}
