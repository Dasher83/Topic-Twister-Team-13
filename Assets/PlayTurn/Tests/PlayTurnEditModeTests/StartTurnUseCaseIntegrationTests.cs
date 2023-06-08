using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.Utils;


public class StartTurnUseCaseIntegrationTests
{
    private IStartTurnUseCase _useCase;
    private IUserMatchesRepository _userMatchesRepository;
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IdaoMapper<Turn, TurnDaoJson> _turnDaoJsonMapper;
    private IdaoMapper<Match, MatchDaoJson> _matchDaoJsonMapper;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
    private IdaoMapper<UserMatch, UserMatchDaoJson> _userMatchDaoJsonMapper;
    private IMatchesRepository _matchesRepository;
    private IUniqueIdGenerator _matchesIdGenerator;
    private IUniqueIdGenerator _roundsIdGenerator;
    private IdaoMapper<Category, CategoryDaoJson> _categoryDaoJsonMapper;
    private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
    private IRoundsRepository _roundsRepository;
    private ITurnsRepository _turnsRepository;
    private IdtoMapper<Turn, TurnDto> _turnDtoMapper;

    [SetUp]
    public void SetUp()
    {
        _matchDaoJsonMapper = new MatchDaoJsonMapper();

        _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
            resourceName: "TestData/Matches",
            matchDaoMapper: _matchDaoJsonMapper);

        _usersReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();

        _matchesIdGenerator = new MatchesIdGenerator(_matchesReadOnlyRepository);

        _matchesRepository = new MatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesIdGenerator: _matchesIdGenerator,
            matchDaoMapper: _matchDaoJsonMapper);

        _userMatchDaoJsonMapper = new UserMatchDaoJsonMapper(
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userReadOnlyRepository: _usersReadOnlyRepository);

        _userMatchesRepository = new UserMatchesRepositoryJson(
            resourceName: "TestData/UserMatches",
            userMatchDaoMapper: _userMatchDaoJsonMapper);

        _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
            resourceName: "TestData/Categories",
            categoryDaoJsonMapper: _categoryDaoJsonMapper);

        _roundsReadOnlyRepository = new RoundsReadOnlyRepositoryJson(
            resourceName: "TestData/Rounds",
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

        _turnDaoJsonMapper = new TurnDaoJsonMapper(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _turnsRepository = new TurnsRepositoryJson(
            resourceName: "TestData/Turns",
            turnDaoMapper: _turnDaoJsonMapper);

        _turnDtoMapper = new TurnDtoMapper();

        _useCase = new StartTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsRepository: _turnsRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository,
            turnDtoMapper: _turnDtoMapper);

        _roundsIdGenerator = new RoundsIdGenerator(roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _roundsRepository = new RoundsRepositoryJson(
            resourceName: "TestData/Rounds",
            roundsIdGenerator: _roundsIdGenerator,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            categoriesReadOnlyRepository: _categoriesReadOnlyRepository);
    }

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
        new RoundsDeleteJson().Delete();
        new TurnsDeleteJson().Delete();
    }

    [Test]
    public void Test_ok()
    {
        #region -- Arrange --
        int userWithIniciativeId = 0;
        int userWithoutIniciativeId = 1;

        User userWithIniciative = _usersReadOnlyRepository.Get(id: userWithIniciativeId).Result;
        User userWithoutIniciative = _usersReadOnlyRepository.Get(id: userWithoutIniciativeId).Result;

        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        UserMatch userWithIniciativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: userWithIniciative,
            match: match);

        _userMatchesRepository.Insert(userWithIniciativeMatch);

        UserMatch userWithoutIniciativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: false,
            user: userWithoutIniciative,
            match: match);

        _userMatchesRepository.Insert(userWithoutIniciativeMatch);

        Round activeRound = new Round(
            roundNumber: 0,
            initialLetter: 'f',
            isActive: true,
            match: match,
            categories: new List<Category>());

        activeRound = _roundsRepository.Insert(activeRound).Result;
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userWithIniciativeId, matchId: match.Id);
        #endregion

        #region -- Assert --
        TurnDto expectedTurnDto = new TurnDto(
            userId: userWithIniciative.Id,
            roundId: activeRound.Id,
            points: 0,
            startDateTime: DateTime.UtcNow,
            endDateTime: null);

        Assert.IsTrue(useCaseOperation.WasOk);
        Assert.AreEqual(expected: expectedTurnDto, actual: useCaseOperation.Result);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;
        int matchId = -1;
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User not found with id: {userId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_match()
    {
        #region -- Arrange --
        int userId = 0;
        int matchId = -1;
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Match not found with id: {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_user_not_in_match()
    {
        #region -- Arrange --
        int userId = 0;
        Match match = new Match();
        Operation<Match> saveMatchOperation = _matchesRepository.Insert(match);
        match = saveMatchOperation.Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'A',
            isActive: true,
            match: match,
            categories: new List<Category>());

        round = _roundsRepository.Insert(round).Result;
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userId, matchId: round.Match.Id);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User with id {userId} is not involved in match with id {match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_already_created_turn()
    {
        #region -- Arrange --
        int userId = 0;
        Match match = new Match();
        Operation<Match> saveMatchOperation = _matchesRepository.Insert(match);
        match = saveMatchOperation.Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'A',
            isActive: true,
            match: match,
            categories: new List<Category>());

        round = _roundsRepository.Insert(round).Result;

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: new List<Round>() { round });

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: _usersReadOnlyRepository.Get(id: userId).Result,
            match: match);

        userMatch = _userMatchesRepository.Insert(userMatch).Result;

        Turn turn = new Turn(
            user: _usersReadOnlyRepository.Get(id: userId).Result,
            round: round,
            startDateTime: DateTime.UtcNow);

        Operation<Turn> insertTurnOperation = _turnsRepository.Insert(turn);
        turn = insertTurnOperation.Result;
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(
            userId: turn.User.Id,
            matchId: userMatch.Match.Id);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);

        string expectedMessage =
            $"Turn already exists for user with id {userId} " +
            $"in round with id {match.ActiveRound.Id} " +
            $"in match with id {match.Id}";

        Assert.AreEqual(expected: expectedMessage, actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_user_turn_order()
    {
        #region -- Arrange --
        int userWithIniciativeId = 1;
        int userWithoutIniciativeId = 2;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'G',
            isActive: true,
            match: match,
            categories: new List<Category>());

        round = _roundsRepository.Insert(round).Result;
        User userWithoutIniciative = _usersReadOnlyRepository.Get(userWithoutIniciativeId).Result;

        UserMatch userWithoutIniciativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: false,
            user: userWithoutIniciative,
            match: match);
        userWithoutIniciativeMatch = _userMatchesRepository.Insert(userWithoutIniciativeMatch).Result;

        User userWithIniciative = _usersReadOnlyRepository.Get(userWithIniciativeId).Result;

        UserMatch userWithIniciativeMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: userWithIniciative,
            match: match);
        userWithIniciativeMatch = _userMatchesRepository.Insert(userWithIniciativeMatch).Result;
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(
            userId: userWithoutIniciative.Id, matchId: match.Id);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn can not be created for user with id {userWithoutIniciativeMatch.User.Id} " +
                $"in round with id {round.Id} in match with id {match.Id} " +
                $"since user with id {userWithIniciativeMatch.User.Id} has not finished his turn yet",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}
