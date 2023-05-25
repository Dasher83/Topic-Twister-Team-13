using System;
using System.Collections.Generic;
using TopicTwister.PlayTurn.Commands;
using TopicTwister.PlayTurn.Services;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;


namespace TopicTwister.PlayTurn.Shared.Providers
{
    public class CommandProvider<Command> 
    {
        private readonly Dictionary<Type, object> _commands;

        private IStartTurnGatewayService _startTurnGatewayService;
        private IStartTurnUseCase _startTurnUseCase;
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

            _commands = new Dictionary<Type, object>
            {
                {
                    typeof(StartTurnCommand),
                    new StartTurnCommand(gatewayService: _startTurnGatewayService)
                },
                {
                    typeof(EndTurnCommand),
                    new EndTurnCommand()
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
