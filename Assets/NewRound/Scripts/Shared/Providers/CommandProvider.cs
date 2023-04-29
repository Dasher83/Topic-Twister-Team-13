using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Commands;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Services;
using TopicTwister.NewRound.Shared.Mappers;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;


namespace TopicTwister.NewRound.Shared.Providers
{
    public class CommandProvider<Command, Presenter> where Command : ICommand<Presenter>
    {
        private readonly Dictionary<Type, object> _actions = new()
        {
            {
                typeof(CreateRoundCommand),
                new CreateRoundCommand(
                    gatewayService: new CreateRoundGatewayService(
                        useCase: new CreateRoundUseCase(
                            roundsRepository: new RoundsRespositoryJson(),
                            matchesRepository: new MatchesRepositoryJson(
                                matchesResourceName: "DevelopmentData/Matches",
                                idGenerator: new MatchesIdGenerator(
                                    matchesRepository: new MatchesReadOnlyRepositoryJson(
                                        matchesResourceName: "DevelopmentData/Matches"))),
                            roundWithCategoriesDtoMapper: new RoundWithCategoriesDtoMapper())))
            }
        };

        public Command Provide()
        {
            _actions.TryGetValue(typeof(Command), out object action);
            return (Command)action;
        }
    }
}
