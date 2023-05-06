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
                return Operation<User>.Success(result: new User(id: userId));
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
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                return Operation<Match>.Success(result: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int matchId = (int)args[1];
                return Operation<UserMatch>.Failure(errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
            });

        _roundsReadOnlyRepository.GetMany(Arg.Any<int>()).Returns(
            (args) =>
            {
                Match match = _matchesReadOnlyRepository.Get(matchId).Result;
                List<Round> rounds = new List<Round>()
                {
                    new Round(
                        id: 0,
                        roundNumber: 0,
                        initialLetter: 'A',
                        isActive: true,
                        match: match,
                        categories: new List<Category>())
                };
                return Operation<List<Round>>.Success(result: rounds);
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
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                return Operation<Match>.Success(result: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int matchId = (int)args[1];
                return Operation<UserMatch>.Success(result: new UserMatch(
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
                return Operation<Round>.Success(result: round);
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
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsReadOnlyRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int roundId = (int)args[1];

                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(userId).Result,
                    round: _roundsReadOnlyRepository.Get(roundId).Result,
                    startDateTime: DateTime.UtcNow);

                return Operation<Turn>.Success(result: turn);
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
        #region -- Arrange --

        int realTestUserId = 1;
        int botId = 2;
        int matchId = 0;
        int roundId = 1;

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                return Operation<Match>.Success(result: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(botId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Success(result: new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    new User(botId),
                    new Match(id: matchId, startDateTime: DateTime.UtcNow))
                );
            });

        _userMatchesRepository.Get(realTestUserId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Success(result: new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    new User(realTestUserId),
                    new Match(id: matchId, startDateTime: DateTime.UtcNow))
                );
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(botId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(realTestUserId, matchId).Result;

                return Operation<UserMatch[]>.Success(result: userMatches);
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
                return Operation<Round>.Success(result: round);
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
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsReadOnlyRepository.Get(Arg.Any<int>(), roundId).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<Turn>.Failure(errorMessage: $"Turn not found with userId: {userId} and roundId: {roundId}");
            });
        #endregion

        #region -- Act --
        Operation<bool> useCaseOperation = _useCase.Execute(userId: botId, matchId: matchId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn can not be created for user with id {botId} " +
                $"in round with id {roundId} in match with id {matchId} " +
                $"since user with id {realTestUserId} has not finished his turn yet",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}
