using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Commands;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Shared.Providers
{
    public class CommandProvider<Command, Presenter> where Command : ICommand<Presenter>
    {
        private readonly Dictionary<Type, object> _actions = new()
        {
            {
                typeof(CreateRoundCommand),
                new CreateRoundCommand()
            }
        };

        public Command Provide()
        {
            _actions.TryGetValue(typeof(Command), out object action);
            return (Command)action;
        }
    }
}
