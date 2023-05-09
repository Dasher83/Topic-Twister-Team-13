using System;
using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Repositories.IdGenerators;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.Utils;


public class EndTurnUseCaseIntegrationTests
{
    private IEndTurnUseCase _useCase;
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IdaoMapper<Match, MatchDaoJson> _matchDaoJsonMapper;
    private IUserMatchesRepository _userMatchesRepository;
    private IdaoMapper<UserMatch, UserMatchDaoJson> _userMatchDaoMapper;
    private IMatchesRepository _matchesRepository;
    private IUniqueIdGenerator _matchesIdGenerator;
    private ITurnsRepository _turnsRepository;
    private IdaoMapper<Turn, TurnDaoJson> _turnDaoMapper;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;
    private ICategoriesReadOnlyRepository _categoriesReadOnlyRepository;
    private IdaoMapper<Category, CategoryDaoJson> _categoryDaoJsonMapper;
    private IRoundsRepository _roundsRepository;
    private IUniqueIdGenerator _roundsIdGenerator;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();

        _matchDaoJsonMapper = new MatchDaoJsonMapper();

        _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
            resourceName: "TestData/Matches",
            matchDaoMapper: _matchDaoJsonMapper);

        _userMatchDaoMapper = new UserMatchDaoJsonMapper(
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userReadOnlyRepository: _usersReadOnlyRepository);

        _userMatchesRepository = new UserMatchesRepositoryJson(
            resourceName: "TestData/UserMatches",
            userMatchDaoMapper: _userMatchDaoMapper);

        _categoryDaoJsonMapper = new CategoryDaoJsonMapper();

        _categoriesReadOnlyRepository = new CategoriesReadOnlyRepositoryJson(
            resourceName: "TestData/Categories",
            categoryDaoJsonMapper: _categoryDaoJsonMapper);

        _roundsReadOnlyRepository = new RoundsReadOnlyRepositoryJson(
            resourceName: "TestData/Rounds",
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            categoriesReadOnlyRepository: _categoriesReadOnlyRepository);

        _turnDaoMapper = new TurnDaoJsonMapper(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _turnsRepository = new TurnsRepositoryJson(
            resourceName: "TestData/Turns",
            turnDaoMapper: _turnDaoMapper);

        _useCase = new EndTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsRepository: _turnsRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

        _matchesIdGenerator = new MatchesIdGenerator(
            matchesReadOnlyRepository: _matchesReadOnlyRepository);

        _matchesRepository = new MatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesIdGenerator: _matchesIdGenerator,
            matchDaoMapper: _matchDaoJsonMapper);

        _roundsIdGenerator = new RoundsIdGenerator(
            roundsReadOnlyRepository: _roundsReadOnlyRepository);

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
    public void Test_ok_for_user_with_iniciative_inside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_with_iniciative_outside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_inside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_outside_time_limit()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;
        int matchId = -1;
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
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
        int userId = Configuration.TestUserId;
        int matchId = -1;
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
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
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(
                userId: user.Id,
                matchId: match.Id,
                answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User with id {user.Id} is not involved in match with id {match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'P',
            isActive: true,
            match: match,
            categories: new List<Category>());

        _roundsRepository.Insert(round);

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: _roundsReadOnlyRepository.GetMany(match.Id).Result);

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: user,
            match: match);
        userMatch = _userMatchesRepository.Insert(userMatch).Result;
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(
            userId: userMatch.User.Id,
            matchId: userMatch.Match.Id,
            answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);

        Assert.AreEqual(
            expected: $"Turn not found for user with id {user.Id} " +
                $"in round with id {match.ActiveRound.Id} in match with id {match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_already_ended_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        User user = _usersReadOnlyRepository.Get(userId).Result;
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        Round round = new Round(
            roundNumber: 0,
            initialLetter: 'P',
            isActive: true,
            match: match,
            categories: new List<Category>());

        _roundsRepository.Insert(round);

        match = new Match(
            id: match.Id,
            startDateTime: match.StartDateTime,
            endDateTime: match.EndDateTime,
            rounds: _roundsReadOnlyRepository.GetMany(match.Id).Result);

        UserMatch userMatch = new UserMatch(
            score: 0,
            isWinner: false,
            hasInitiative: true,
            user: user,
            match: match);
        userMatch = _userMatchesRepository.Insert(userMatch).Result;

        Turn turn = new Turn(
            user: user,
            round: match.ActiveRound,
            startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds),
            endDateTime: DateTime.UtcNow);

        turn = _turnsRepository.Insert(turn).Result;
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(
                userId: userMatch.User.Id,
                matchId: userMatch.Match.Id,
                answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn already ended for user with id {turn.User.Id} " +
                    $"in round with id {turn.Round.Id} in match with id {turn.Round.Match.Id}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_too_many_or_too_few_answers()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_category_mismatch()
    {
        throw new NotImplementedException();
    }
}
