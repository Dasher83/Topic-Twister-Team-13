using System;
using System.Collections.Generic;
using TopicTwister.Home.Scripts.Commands;
using TopicTwister.Home.Services;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.Scripts.Shared.Providers
{
    public class CommandProvider<T> where T : ICommand
    {
        private readonly Dictionary<Type, object> _commands = new()
        {
            { 
                typeof(CreateNewBotMatchCommand),
                    new CreateNewBotMatchCommand(
                        new CreateBotMatchService()) 
            }
        };

        public T Provide()
        {
            _commands.TryGetValue(typeof(T), out object command);
            return (T)command;
        }
    }
}
