using System;
using System.Collections.Generic;
using TopicTwister.PlayTurn.Commands;
using TopicTwister.PlayTurn.Services;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.PlayTurn.UseCases;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;


namespace TopicTwister.PlayTurn.Shared.Providers
{
    public class CommandProvider<Command> 
    {
        private readonly Dictionary<Type, object> _commands;

        private IStartTurnGatewayService _startTurnGatewayService;
        private IStartTurnUseCase _startTurnUseCase;
        private IEndTurnGatewayService _endTurnGatewayService;
        private IEndTurnUseCase _endTurnUseCase;
        private IUsersReadOnlyRepository _usersReadOnlyRepository;
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;
        private IUserMatchesRepository _userMatchesRepository;
        private IdaoMapper<UserMatch, UserMatchDaoJson> _userMatchDaoJsonMapper;
        private ITurnsRepository _turnsRepository;
        private IdaoMapper<Turn, TurnDaoJson> _turnDaoJsonMapper;
        private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
        private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
        private IdaoMapper<Category, CategoryDaoJson> _categoryDaoJsonMapper;
        private IdtoMapper<Turn, TurnDto> _turnDtoMapper;

        private IRoundsRepository _roundsRepository;
        private IUniqueIdGenerator _roundsIdGenerator;
        private IUniqueIdGenerator _matchesIdGenerator;
        private IMatchesRepository _matchesRepository;
        private IdtoMapper<Round, RoundWithCategoriesDto> _roundWithCategoriesDtoMapper;
        private IWordsRepository _wordsRepository;
        private IAnswersRepository _answersRepository;
        private ITurnsReadOnlyRepository _turnsReadOnlyRepository;
        private IUserRoundsRepository _userRoundsRepository;
        private IdtoMapper<Category, CategoryDto> _categoryDtoMapper;
        private IdtoMapper<Round, RoundDto> _roundDtoMapper;
        private IdtoMapper<Match, MatchDto> _matchDtoMapper;
        private IdtoMapper<UserMatch, UserMatchDto> _userMatchDtoMapper;
        private IdtoMapper<Answer, AnswerDto> _answerDtoMapper;
        private IdtoMapper<UserRound, UserRoundDto> _userRoundDtoMapper;
        private IdaoMapper<Answer, AnswerDaoJson> _answerDaoMapper;
        private IdaoMapper<UserRound, UserRoundDaoJson> _userRoundDaoJsonMapper;

        public CommandProvider()
        {
            _matchDaoMapper = new MatchDaoJsonMapper();

            _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Matches",
                matchDaoMapper: _matchDaoMapper);

            _usersReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();

            _userMatchDaoJsonMapper = new UserMatchDaoJsonMapper(
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                userReadOnlyRepository: _usersReadOnlyRepository);

            _userMatchesRepository = new UserMatchesRepositoryJson(
                resourceName: "DevelopmentData/UserMatches",
                userMatchDaoMapper: _userMatchDaoJsonMapper);

            _categoryDaoJsonMapper = new CategoryDaoJsonMapper();

            _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Categories",
                categoryDaoJsonMapper: _categoryDaoJsonMapper);

            _roundsReadOnlyRepository = new RoundsReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Rounds",
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

            _turnDaoJsonMapper = new TurnDaoJsonMapper(
                usersReadOnlyRepository: _usersReadOnlyRepository,
                roundsReadOnlyRepository: _roundsReadOnlyRepository);

            _turnsRepository = new TurnsRepositoryJson(
                resourceName: "DevelopmentData/Turns",
                turnDaoMapper: _turnDaoJsonMapper);

            _turnDtoMapper = new TurnDtoMapper();

            _startTurnUseCase = new StartTurnUseCase(
                usersReadOnlyRepository: _usersReadOnlyRepository,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                userMatchesRepository: _userMatchesRepository,
                turnsRepository: _turnsRepository,
                roundsReadOnlyRepository: _roundsReadOnlyRepository,
                turnDtoMapper: _turnDtoMapper);

            _startTurnGatewayService = new StartTurnGatewayService(useCase: _startTurnUseCase);
            _roundsIdGenerator = new RoundsIdGenerator(roundsReadOnlyRepository: _roundsReadOnlyRepository);

            _roundsRepository = new RoundsRepositoryJson(
                resourceName: "DevelopmentData/Rounds",
                roundsIdGenerator: _roundsIdGenerator,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

            _categoryDtoMapper = new CategoryDtoMapper();
            _roundDtoMapper = new RoundDtoMapper();

            _roundWithCategoriesDtoMapper = new RoundWithCategoriesDtoMapper(
                categoryDtoMapper: _categoryDtoMapper,
                roundDtoMapper: _roundDtoMapper,
                roundReadOnlyRepository: _roundsReadOnlyRepository);

            _matchesIdGenerator = new MatchesIdGenerator(matchesReadOnlyRepository: _matchesReadOnlyRepository);

            _matchesRepository = new MatchesRepositoryJson(
                resourceName: "DevelopmentData/Matches",
                matchesIdGenerator: _matchesIdGenerator,
                matchDaoMapper: _matchDaoMapper);

            _userMatchDtoMapper = new UserMatchDtoMapper(
                matchesRepository: _matchesRepository,
                usersReadOnlyRepository: _usersReadOnlyRepository);

            _wordsRepository = new WordsRepositoryJson(resourceName: "DevelopmentData/Words");

            _answerDtoMapper = new AnswerDtoMapper(
                categoryDtoMapper: _categoryDtoMapper,
                wordsRepository: _wordsRepository);

            _turnsReadOnlyRepository = new TurnsReadOnlyRepositoryJson(
                resourceName: "DevelopmentData/Turns",
                turnDaoMapper: _turnDaoJsonMapper);

            _answerDaoMapper = new AnswerDaoJsonMapper(
                categoriesReadOnlyRepository: _categoriesReadOnlyRepository,
                turnsReadOnlyRepository: _turnsReadOnlyRepository);

            _answersRepository = new AnswersRepositoryJson(
                resourceName: "DevelopmentData/Answers",
                daoMapper: _answerDaoMapper);

            _userRoundDtoMapper = new UserRoundDtoMapper();
            _matchDtoMapper = new MatchDtoMapper();

            _userRoundDaoJsonMapper = new UserRoundDaoJsonMapper(
                usersReadOnlyRepository: _usersReadOnlyRepository,
                roundsReadOnlyRepository: _roundsReadOnlyRepository);

            _userRoundsRepository = new UserRoundsRepository(
                resourceName: "DevelopmentData/UserRounds",
                userRoundDaoJsonMapper: _userRoundDaoJsonMapper);

            _endTurnUseCase = new EndTurnUseCase(
                usersReadOnlyRepository: _usersReadOnlyRepository,
                matchesReadOnlyRepository: _matchesReadOnlyRepository,
                userMatchesRepository: _userMatchesRepository,
                turnsRepository: _turnsRepository,
                roundsRepository: _roundsRepository,
                matchDtoMapper: _matchDtoMapper,
                roundWithCategoriesDtoMapper: _roundWithCategoriesDtoMapper,
                userMatchDtoMapper: _userMatchDtoMapper,
                answerDtoMapper: _answerDtoMapper,
                answersRepository: _answersRepository,
                wordsRepository: _wordsRepository,
                userRoundsRepository: _userRoundsRepository,
                userRoundDtoMapper: _userRoundDtoMapper);

            _endTurnGatewayService = new EndTurnGategayService(useCase: _endTurnUseCase);

            _commands = new Dictionary<Type, object>
            {
                {
                    typeof(StartTurnCommand),
                    new StartTurnCommand(gatewayService: _startTurnGatewayService)
                },
                {
                    typeof(EndTurnCommand),
                    new EndTurnCommand(gatewayService: _endTurnGatewayService)
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
