using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


public class StartTurnUseCaseUnitTests
{
    IStartTurnUseCase _useCase;
    IUsersReadOnlyRepository _usersReadOnlyRepository;
    IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    IUserMatchesRepository _userMatchesRepository;
    ITurnsReadOnlyRepository _turnsReadOnlyRepository;
    IRoundsReadOnlyRepository _roundsReadOnlyRepository;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = Substitute.For<IUsersReadOnlyRepository>();
        _matchesReadOnlyRepository = Substitute.For<IMatchesReadOnlyRepository>();
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _turnsReadOnlyRepository = Substitute.For<ITurnsReadOnlyRepository>();
        _roundsReadOnlyRepository = Substitute.For<IRoundsReadOnlyRepository>();

        _useCase = new StartTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsReadOnlyRepository: _turnsReadOnlyRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);
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

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
            });
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

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(outcome: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                return Operation<Match>.Failure(errorMessage: $"Match not found with id: {matchId}");
            });
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
        int matchId = 0;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(outcome: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                return Operation<Match>.Success(outcome: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int matchId = (int)args[1];
                return Operation<UserMatch>.Failure(errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
            });
        #endregion

        #region -- Act --
        Operation<bool> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User with id {userId} is not involved in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_already_created_turn()
    {
        #region -- Arrange --

        int userId = 0;
        int matchId = 0;
        int roundId = 1;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(outcome: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                return Operation<Match>.Success(outcome: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int matchId = (int)args[1];
                return Operation<UserMatch>.Success(outcome: new UserMatch(
                    score: 2, 
                    isWinner: true, 
                    hasInitiative: true, 
                    new User(userId), 
                    new Match(id: matchId, startDateTime: DateTime.UtcNow))
                );
            });

        _roundsReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int roundId = (int)args[0];
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: new Match(id: matchId, startDateTime: DateTime.UtcNow),
                    categories: new List<Category>());
                return Operation<Round>.Success(outcome: round);
            });

        _roundsReadOnlyRepository.GetMany(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                List<Round> rounds = new List<Round>();
                Round activeRound = new Round(
                    id: roundId, 
                    roundNumber: 0, 
                    initialLetter: 'f', 
                    isActive: true,
                    match: new Match(id: matchId, startDateTime: DateTime.UtcNow), 
                    categories: new List<Category>());
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(outcome: rounds);
            });

        _turnsReadOnlyRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int roundId = (int)args[1];

                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(userId).Outcome,
                    round: _roundsReadOnlyRepository.Get(roundId).Outcome,
                    startDateTime: DateTime.UtcNow);

                return Operation<Turn>.Success(outcome: turn);
            });
        #endregion

        #region -- Act --
        Operation<bool> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn already exists for user with id {userId} in round with id {roundId} in match with id {matchId}", 
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_user_turn_order()
    {
        throw new NotImplementedException();
    }
}
