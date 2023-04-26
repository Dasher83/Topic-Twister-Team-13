using System;
using System.Collections.Generic;
using TopicTwister.Home.Commands;
using TopicTwister.Home.Services;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;


namespace TopicTwister.Home.Shared.Providers
{
    public class CommandProvider<Command, Presenter> where Command : ICommand<Presenter>
    {
        private readonly Dictionary<Type, object> _commands = new()
        {
            {
                typeof(CreateNewBotMatchCommand),
                    new CreateNewBotMatchCommand(
                        new CreateBotMatchService(
                            new CreateBotMatchUseCase(
                                new MatchesRepositoryJson(
                                    matchesResourceName: "DevelopmentData/Matches",
                                    idGenerator: new MatchesIdGenerator(
                                        matchesRepository: new MatchesReadOnlyRepositoryJson(
                                        matchesResourceName: "DevelopmentData/Matches"))),
                                new UserMatchesRepositoryJson(
                                    userMatchesResourceName: "DevelopmentData/UserMatches",
                                    matchesRepository: new MatchesRepositoryJson(
                                        matchesResourceName: "DevelopmentData/Matches",
                                        idGenerator: new MatchesIdGenerator(
                                            matchesRepository: new MatchesReadOnlyRepositoryJson(
                                            matchesResourceName: "DevelopmentData/Matches"))),
                                    userRepository: new UsersRepositoryInMemory()),
                                new UsersRepositoryInMemory(),
                                new MatchDtoMapper())))
            }
        };

        public Command Provide()
        {
            _commands.TryGetValue(typeof(Command), out object command);
            return (Command)command;
        }
    }
}
