using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Actions;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.ResultRound.Shared.Providers
{
    public class ActionProvider<T> where T : IAction
    {
        private readonly Dictionary<Type, object> _actions = new()
        {
            {
                typeof(EvaluateAnswersAction),
                new EvaluateAnswersAction(/*Todo: complete dependency inyection*/)
            }
        };

        public T Provide()
        {
            _actions.TryGetValue(typeof(T), out object action);
            return (T)action;
        }
    }
}
