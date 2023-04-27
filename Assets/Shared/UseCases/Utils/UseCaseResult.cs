namespace TopicTwister.Shared.UseCases.Utils
{
    public class UseCaseResult<T>
    {
        private readonly T _outcome;
        private readonly string _errorMessage;
        private readonly bool _wasOk;

        public T Outcome { get => _outcome; }
        public string ErrorMessage { get => _errorMessage; }
        public bool WasOk { get => _wasOk; }

        private UseCaseResult(T outcome, string errorMessage, bool wasOk)
        {
            _outcome = outcome;
            _errorMessage = errorMessage;
            _wasOk = wasOk;
        }

        public static UseCaseResult<T> Success(T outcome)
        {
            return new UseCaseResult<T>(outcome, null, true);
        }

        public static UseCaseResult<T> Failure(string errorMessage)
        {
            return new UseCaseResult<T>(default, errorMessage, false);
        }
    }
}
