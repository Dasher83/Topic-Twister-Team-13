using System;


namespace TopicTwister.Shared.Utils
{
    public class Operation<T>
    {
        private readonly T _result;
        private readonly string _errorMessage;
        private readonly bool _wasOk;

        public T Result
        {
            get
            {
                if (_wasOk == false)
                {
                    string message = "Cannot access Outcome property when the operation was not successful.";
                    throw new InvalidOperationException(message);
                }

                return _result;
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

        private Operation(T result, string errorMessage, bool wasOk)
        {
            _result = result;
            _errorMessage = errorMessage;
            _wasOk = wasOk;
        }

        public static Operation<T> Success(T result)
        {
            return new Operation<T>(result, null, true);
        }

        public static Operation<T> Failure(string errorMessage)
        {
            return new Operation<T>(default, errorMessage, false);
        }
    }
}
