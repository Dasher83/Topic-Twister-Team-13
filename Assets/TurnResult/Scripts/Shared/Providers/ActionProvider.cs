using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Actions;
using TopicTwister.Shared.Interfaces;
using TopicTwister.TurnResult.Services;


namespace TopicTwister.TurnResult.Shared.Providers
{
    public class ActionProvider<T> where T : IAction
    {
        private readonly Dictionary<Type, object> _actions = new()
        {
            {
                typeof(EvaluateAnswersAction),
                new EvaluateAnswersAction(new AnswersEvaluationServiceFake())
            }
        };

        public T Provide()
        {
            _actions.TryGetValue(typeof(T), out object action);
            return (T)action;
        }
    }
}
