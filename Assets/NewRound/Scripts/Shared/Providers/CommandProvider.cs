using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Commands;
using TopicTwister.NewRound.Repositories;
using TopicTwister.NewRound.Repositories.IdGenerators;
using TopicTwister.NewRound.Services;
using TopicTwister.NewRound.Shared.Interfaces;
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
                            roundsRepository: new RoundsRespositoryJson(
                                resourceName: "DevelopmentData/Rounds",
                                idGenerator: new RoundsIdGenerator(
                                    roundsRepository: new RoundsReadOnlyRepositoryJson(
                                        resourceName: "DevelopmentData/Rounds",
                                        matchesRepository: new MatchesReadOnlyRepositoryJson(
                                            resourceName: "DevelopmentData/Matches",
                                            matchDaoMapper: new MatchDaoJsonMapper()),
                                        categoriesRepository: new CategoriesReadOnlyRepositoryJson(
                                            categoriesResourceName: "DevelopmentData/Categories",
                                            mapper: new CategoryDaoJsonMapper()))),
                                matchesRepository: new MatchesReadOnlyRepositoryJson(
                                            resourceName: "DevelopmentData/Matches",
                                            matchDaoMapper: new MatchDaoJsonMapper()),
                                categoriesRepository: new CategoriesReadOnlyRepositoryJson(
                                            categoriesResourceName: "DevelopmentData/Categories",
                                            mapper: new CategoryDaoJsonMapper())),
                            matchesRepository: new MatchesRepositoryJson(
                                    matchesResourceName: "DevelopmentData/Matches",
                                    idGenerator: new MatchesIdGenerator(
                                        matchesRepository: new MatchesReadOnlyRepositoryJson(
                                            resourceName: "DevelopmentData/Matches",
                                            matchDaoMapper: new MatchDaoJsonMapper())),
                                    matchDaoMapper: new MatchDaoJsonMapper()),
                            categoryRepository: new CategoriesReadOnlyRepositoryJson(
                                categoriesResourceName: "DevelopmentData/Categories",
                                mapper: new CategoryDaoJsonMapper()),
                            letterRepository: new LetterReadOnlyRepositoryInMemory(),
                            roundWithCategoriesDtoMapper: new RoundWithCategoriesDtoMapper(
                                categoryDtoMapper: new CategoryDtoMapper(),
                                roundDtoMapper: new RoundDtoMapper()))))
            }
        };

        public Command Provide()
        {
            _actions.TryGetValue(typeof(Command), out object action);
            return (Command)action;
        }
    }
}
