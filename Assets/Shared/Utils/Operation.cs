using System;


namespace TopicTwister.Shared.Utils
{
    public class Operation<T>
    {
        private readonly T _outcome;
        private readonly string _errorMessage;
        private readonly bool _wasOk;

        public T Outcome
        {
            get
            {
                if (_wasOk == false)
                {
                    string message = "Cannot access Outcome property when the operation was not successful.";
                    throw new InvalidOperationException(message);
                }

                return _outcome;
            }
        }

        public string ErrorMessage
        {
            get
            {
                if(_wasOk) throw new InvalidOperationException(
                    "Cannot access ErrorMessage property when the operation was successful.");

                return _errorMessage;
            }
        }

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
