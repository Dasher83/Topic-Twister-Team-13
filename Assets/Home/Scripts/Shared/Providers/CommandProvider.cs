using System;
using System.Collections.Generic;
using TopicTwister.Home.Scripts.Commands;
using TopicTwister.Home.Services;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Repositories;


namespace TopicTwister.Home.Scripts.Shared.Providers
{
    public class CommandProvider<T> where T : ICommand
    {
        private readonly Dictionary<Type, object> _commands = new()
        {
            {
                typeof(CreateNewBotMatchCommand),
                    new CreateNewBotMatchCommand(
                        new CreateBotMatchService(
                            new CreateBotMatchUseCase(
                                new MatchesRepositoryJson(
                                    matchesResourceName: "DevelopmentData/Matches"),
                                new UserMatchesRepositoryJson(
                                    userMatchesResourceName: "DevelopmentData/UserMatches",
                                    matchesRepository: new MatchesRepositoryJson(matchesResourceName: "DevelopmentData/Matches")),
                                new UserRepositoryInMemory(),
                                new MatchDtoMapper())))
            }
        };

        public T Provide()
        {
            _commands.TryGetValue(typeof(T), out object command);
            return (T)command;
        }
    }
}
