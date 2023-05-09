using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Commands;
using TopicTwister.Shared.Interfaces;
using TopicTwister.TurnResult.Services;
using TopicTwister.TurnResult.UseCases;
using TopicTwister.TurnResult.Repositories;
using TopicTwister.TurnResult.Shared.Interfaces;


namespace TopicTwister.TurnResult.Shared.Providers
{
    public class CommandProvider<Command> where Command : ICommand<ITurnResultPresenter>
    {
        private readonly Dictionary<Type, object> _commands = new()
        {
            {
                typeof(EvaluateAnswersCommand),
                new EvaluateAnswersCommand(
                    new AnswersEvaluationService(
                        new EvaluateAnswersUseCase(
                            new WordsRepositoryJson("JSON/TestData/WordsTest"))))
            }
        };

        public Command Provide()
        {
            _commands.TryGetValue(typeof(Command), out object command);
            return (Command)command;
        }
    }
}
