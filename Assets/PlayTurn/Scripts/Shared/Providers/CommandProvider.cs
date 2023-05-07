using System;
using System.Collections.Generic;
using TopicTwister.PlayTurn.Commands;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Shared.Providers
{
    public class CommandProvider<Command> where Command : ICommand<IStartTurnPresenter>
    {
        private readonly Dictionary<Type, object> _commands;

        public CommandProvider()
        {
            _commands = new Dictionary<Type, object>
            {
                {
                    typeof(StartTurnCommand),
                    new StartTurnCommand()
                } 
            };
        }

        public Command Provide()
        {
            _commands.TryGetValue(typeof(Command), out object command);
            return (Command)command;
        }
    }
}
