using System;
using System.Collections.Generic;
using TopicTwister.Home.Commands;
using TopicTwister.Home.Services;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.UseCases;
using TopicTwister.Repositories.IdGenerators;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.UseCases;


namespace TopicTwister.Home.Shared.Providers
{
    public class CommandProvider<Command, Presenter> where Command : ICommand<Presenter>
    {
        private IdtoMapper<Match, MatchDto> _matchDtoMapper;
        private IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private IUniqueIdGenerator _matchesIdGenerator;
        private IMatchesRepository _matchRepository;
        private IUserReadOnlyRepository _userReadOnlyRepository;
        private IUserMatchesRepository _userMatchesRepository;
        private ICreateMatchSubUseCase _createMatchSubUseCase;

        private ICreateRoundSubUseCase _createRoundSubUseCase;

        private IdaoMapper<Category, CategoryDaoJson> _categoryDaoMapper;
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
        private IUniqueIdGenerator _roundsIdGenerator;
        private IRoundsRepository _roundsRepository;
        private ILetterReadOnlyRepository _letterReadOnlyRepository;
        private IdtoMapper<Category, CategoryDto> _categoryDtoMapper;
        private IdtoMapper<Round, RoundDto> _roundDtoMapper;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
        private IStartBotMatchUseCase _createBotMatchUseCase;

        private IStartBotMatchService _createBotMatchService;

        private readonly Dictionary<Type, object> _commands;

        public CommandProvider()
        {
            _matchDtoMapper = new MatchDtoMapper();
            _matchDaoMapper = new MatchDaoJsonMapper();

            _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Matches",
                matchDaoMapper: _matchDaoMapper);

            _matchesIdGenerator = new MatchesIdGenerator(
                matchesReadOnlyRepository: _matchesReadOnlyRepository);

            _matchRepository = new MatchesRepositoryJson(
                resourceName: "DevelopmentData/Matches",
                matchesIdGenerator: _matchesIdGenerator,
                matchDaoMapper: _matchDaoMapper);

            _userReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();

            _userMatchesRepository = new UserMatchesRepositoryJson(
                resourceName: "DevelopmentData/UserMatches",
                matchesRepository: _matchRepository,
                userReadOnlyRepository: _userReadOnlyRepository);

            _createMatchSubUseCase = new CreateMatchSubUseCase(
                matchesRepository: _matchRepository,
                userMatchesRepository: _userMatchesRepository,
                userRespository: _userReadOnlyRepository,
                matchDtoMapper: _matchDtoMapper);

            _categoryDaoMapper = new CategoryDaoJsonMapper();

            _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Category",
                categoryDaoJsonMapper: _categoryDaoMapper);

            _roundsReadOnlyRepository = new RoundsReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Rounds",
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

            _roundsIdGenerator = new RoundsIdGenerator(
                roundsReadOnlyRepository: _roundsReadOnlyRepository);

            _roundsRepository = new RoundsRepositoryJson(
                resourceName: "DevelopmentData/Rounds",
                roundsIdGenerator: _roundsIdGenerator,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

            _letterReadOnlyRepository = new LetterReadOnlyRepositoryInMemory();
            _categoryDtoMapper = new CategoryDtoMapper();
            _roundDtoMapper = new RoundDtoMapper();

            _roundWithCategoriesDtoMapper = new RoundWithCategoriesDtoMapper(
                categoryDtoMapper: _categoryDtoMapper,
                roundDtoMapper: _roundDtoMapper);

            _createRoundSubUseCase = new CreateRoundSubUseCase(
                roundsRepository: _roundsRepository,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoryReadOnlyRepository: _categoriesReadOnlyRepository,
                letterReadOnlyRepository: _letterReadOnlyRepository,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper);

            _createBotMatchUseCase = new StartBotMatchUseCase(
                createMatchSubUseCase: _createMatchSubUseCase,
                createRoundSubUseCase: _createRoundSubUseCase);

            _createBotMatchService = new StartBotMatchService(_createBotMatchUseCase);

            _commands = new Dictionary<Type, object>
            {
                {
                    typeof(StartNewBotMatchCommand),
                    new StartNewBotMatchCommand(_createBotMatchService)
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
