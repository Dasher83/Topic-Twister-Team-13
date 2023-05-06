using System;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.Utils;


public class StartTurnUseCaseIntegrationTests
{
    IStartTurnUseCase _useCase;
    IUserMatchesRepository _userMatchesRepository;
    IUsersReadOnlyRepository _usersReadOnlyRepository;
    IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private ITurnsReadOnlyRepository _turnsReadOnlyRepository;
    IdaoMapper<Match, MatchDaoJson> _matchDaoJsonMapper;
    IRoundsReadOnlyRepository _roundsReadOnlyRepository;

    IMatchesRepository _matchesRepository;
    IUniqueIdGenerator _matchIdGenerator;

    [SetUp]
    public void SetUp()
    {
        _matchDaoJsonMapper = new MatchDaoJsonMapper();

        _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
            resourceName: "TestData/Matches",
            matchDaoMapper: _matchDaoJsonMapper);

        _usersReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();

        _userMatchesRepository = new UserMatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesRepository: _matchesRepository,
            usersReadOnlyRepository: _usersReadOnlyRepository);

        _useCase = new StartTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsReadOnlyRepository: _turnsReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository
            );

        _matchIdGenerator = new MatchesIdGenerator(_matchesReadOnlyRepository);

        _matchesRepository = new MatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesIdGenerator: _matchIdGenerator,
            matchDaoMapper: _matchDaoJsonMapper);
    }

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
        new RoundsDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_scenario()
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
        Operation<bool> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
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
        Operation<bool> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
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
        Operation<Match> saveMatchOperation = _matchesRepository.Save(match);
        match = saveMatchOperation.Outcome;
        #endregion

        #region -- Act --
        Operation<bool> useCaseOperation = _useCase.Execute(userId: userId, matchId: match.Id);
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
        throw new NotImplementedException();
    }

    [Test]
    public void Test_fail_due_to_user_turn_order()
    {
        throw new NotImplementedException();
    }
}
