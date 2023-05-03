namespace TopicTwister.Shared.Utils
{
    public class Operation<T>
    {
        private readonly T _outcome;
        private readonly string _errorMessage;
        private readonly bool _wasOk;

        public T Outcome { get => _outcome; }
        public string ErrorMessage { get => _errorMessage; }
        public bool WasOk { get => _wasOk; }

        private Operation(T outcome, string errorMessage, bool wasOk)
        {
            _outcome = outcome;
            _errorMessage = errorMessage;
            _wasOk = wasOk;
        }

        public static Operation<T> Success(T outcome)
        {
            return new Operation<T>(outcome, null, true);
        }

        public static Operation<T> Failure(string errorMessage)
        {
            return new Operation<T>(default, errorMessage, false);
        }
    }
}
