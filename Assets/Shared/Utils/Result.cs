namespace TopicTwister.Shared.Utils
{
    public class Result<T>
    {
        private readonly T _outcome;
        private readonly string _errorMessage;
        private readonly bool _wasOk;

        public T Outcome { get => _outcome; }
        public string ErrorMessage { get => _errorMessage; }
        public bool WasOk { get => _wasOk; }

        private Result(T outcome, string errorMessage, bool wasOk)
        {
            _outcome = outcome;
            _errorMessage = errorMessage;
            _wasOk = wasOk;
        }

        public static Result<T> Success(T outcome)
        {
            return new Result<T>(outcome, null, true);
        }

        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T>(default, errorMessage, false);
        }
    }
}
