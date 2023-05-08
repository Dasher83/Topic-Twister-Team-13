using System;
using System.Collections.Generic;
using TopicTwister.NewRound.Commands;
using TopicTwister.NewRound.Services;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.UseCases;
using TopicTwister.Repositories.IdGenerators;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.UseCases;


namespace TopicTwister.NewRound.Shared.Providers
{
    public class CommandProvider<Command> where Command : ICommand<IResumeMatchPresenter>
    {
        private IdaoMapper<Match, MatchDaoJson> _matchDaoJsonMapper;
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private IdaoMapper<Category, CategoryDaoJson> _categoryDaoJsonMapper;
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
        private IUniqueIdGenerator _roundsIdGeneration;
        private IRoundsRepository _roundsRepository;
        private ILetterReadOnlyRepository _letterReadOnlyRepository;
        private IdtoMapper<Category, CategoryDto> _categoryDtoMapper;
        private IdtoMapper<Round, RoundDto> _roundDtoMapper;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
        private ICreateRoundSubUseCase _createRoundSubUseCase;
        private IResumeMatchUseCase _resumeMatchUseCase;

        private readonly Dictionary<Type, object> _actions;

        public CommandProvider()
        {
            _matchDaoJsonMapper = new MatchDaoJsonMapper();

            _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Matches",
                matchDaoMapper: _matchDaoJsonMapper);

            _categoryDaoJsonMapper = new CategoryDaoJsonMapper();

            _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Categories",
                categoryDaoJsonMapper: _categoryDaoJsonMapper);

            _roundsReadOnlyRepository = new RoundsReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Rounds",
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

            _roundsIdGeneration = new RoundsIdGenerator(
                roundsReadOnlyRepository: _roundsReadOnlyRepository);

            _roundsRepository = new RoundsRepositoryJson(
                resourceName: "DevelopmentData/Rounds",
                roundsIdGenerator: _roundsIdGeneration,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

            _letterReadOnlyRepository = new LetterReadOnlyRepositoryInMemory();
            _categoryDtoMapper = new CategoryDtoMapper();
            _roundDtoMapper = new RoundDtoMapper();

            _roundWithCategoriesDtoMapper = new RoundWithCategoriesDtoMapper(
                categoryDtoMapper: _categoryDtoMapper,
                roundDtoMapper: _roundDtoMapper,
                roundReadOnlyRepository: _roundsReadOnlyRepository);

            _createRoundSubUseCase = new CreateRoundSubUseCase(
                roundsRepository: _roundsRepository,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterReadOnlyRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);

            _resumeMatchUseCase = new ResumeMatchUseCase(
                createRoundSubUseCase: _createRoundSubUseCase,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                roundsReadOnlyRepository: _roundsReadOnlyRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);

            _actions = new()
            {
                {
                    typeof(ResumeMatchCommand),
                    new ResumeMatchCommand(
                    gatewayService: new ResumeMatchGatewayService(_resumeMatchUseCase))
                }
            };
        }

        public Command Provide()
        {
            _actions.TryGetValue(typeof(Command), out object action);
            return (Command)action;
        }
    }
}
