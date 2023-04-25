using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Actions;
using TopicTwister.Shared.Interfaces;
using TopicTwister.TurnResult.Services;
using TopicTwister.TurnResult.UseCases;
using TopicTwister.TurnResult.Repositories;


namespace TopicTwister.TurnResult.Shared.Providers
{
    public class ActionProvider<Command, Presenter> where Command : ICommand<Presenter>
    {
        private readonly Dictionary<Type, object> _actions = new()
        {
            {
                typeof(EvaluateAnswersAction),
                new EvaluateAnswersAction(
                    new AnswersEvaluationService(
                        new EvaluateAnswersUseCase(
                            new WordsRepositoryJson("JSON/TestData/WordsTest"))))
            }
        };

        public Command Provide()
        {
            _actions.TryGetValue(typeof(Command), out object action);
            return (Command)action;
        }
    }
}
