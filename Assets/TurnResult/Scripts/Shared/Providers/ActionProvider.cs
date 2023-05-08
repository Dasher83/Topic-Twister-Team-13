using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Actions;
using TopicTwister.Shared.Interfaces;
using TopicTwister.TurnResult.Services;
using TopicTwister.TurnResult.UseCases;
using TopicTwister.TurnResult.Repositories;
using TopicTwister.TurnResult.Shared.Interfaces;


namespace TopicTwister.TurnResult.Shared.Providers
{
    public class ActionProvider<Command> where Command : ICommand<ITurnResultPresenter>
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
