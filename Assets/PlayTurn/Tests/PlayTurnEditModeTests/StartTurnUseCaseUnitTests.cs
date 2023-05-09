using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Utils;


public class StartTurnUseCaseUnitTests
{
    IStartTurnUseCase _useCase;
    IUsersReadOnlyRepository _usersReadOnlyRepository;
    IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    IUserMatchesRepository _userMatchesRepository;
    ITurnsRepository _turnsRepository;
    IRoundsReadOnlyRepository _roundsReadOnlyRepository;
    IdtoMapper<Turn, TurnDto> _turnDtoMapper;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = Substitute.For<IUsersReadOnlyRepository>();
        _matchesReadOnlyRepository = Substitute.For<IMatchesReadOnlyRepository>();
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _turnsRepository = Substitute.For<ITurnsRepository>();
        _roundsReadOnlyRepository = Substitute.For<IRoundsReadOnlyRepository>();
        _turnDtoMapper = Substitute.For<IdtoMapper<Turn,TurnDto>>();

        _useCase = new StartTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsRepository: _turnsRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository,
            turnDtoMapper: _turnDtoMapper);
    }

    [Test]
    public void Test_ok()
    {
        #region -- Arrange --
        int userWithIniciativeId = 1;
        int userWithoutIniciativeId = 2;
        int matchId = 0;
        int roundId = 0;

        User userWithIniciative = new User(id: userWithIniciativeId);
        User userWithoutIniciative = new User(id: userWithoutIniciativeId);

        _usersReadOnlyRepository.Get(userWithIniciativeId)
            .Returns((args) => { return Operation<User>.Success(result: userWithIniciative); });

        _usersReadOnlyRepository.Get(userWithoutIniciativeId)
            .Returns((args) => { return Operation<User>.Success(result: userWithoutIniciative); });

        Match match = new Match(
            id: matchId,
            startDateTime: DateTime.UtcNow,
            endDateTime: null);

        _roundsReadOnlyRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());
                return Operation<Round>.Success(result: round);
            });

        match = new Match(
            id: matchId,
            startDateTime: DateTime.UtcNow,
            endDateTime: null,
            rounds: new List<Round>() { _roundsReadOnlyRepository.Get(roundId).Result });

        _matchesReadOnlyRepository.Get(match.Id).Returns(
            (args) => { return Operation<Match>.Success(result: match); });

        _userMatchesRepository.Get(userWithoutIniciativeId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Success(result: new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    user: userWithoutIniciative,
                    match: match)
                );
            });

        _userMatchesRepository.Get(userWithIniciativeId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Success(result: new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    user: userWithIniciative,
                    match: match)
                );
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userWithoutIniciativeId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(userWithIniciativeId, matchId).Result;
                return Operation<UserMatch[]>.Success(result: userMatches);
            });

        _roundsReadOnlyRepository.GetMany(Arg.Any<int>()).Returns(
            (args) =>
            {
                int matchId = (int)args[0];
                List<Round> rounds = new List<Round>() { _roundsReadOnlyRepository.Get(roundId).Result };
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userWithIniciativeId, roundId).Returns(
            (args) =>
            {
                string errorMessage = $"Turn not found with userId: {userWithIniciativeId} and roundId: {roundId}";
                return Operation<Turn>.Failure(errorMessage: errorMessage);
            });

        _turnsRepository.Insert(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn turn = (Turn)args[0];
                return Operation<Turn>.Success(result: turn);
            });

        _turnDtoMapper.ToDTO(Arg.Any<Turn>()).Returns(
            (args) =>
            {
                Turn turn = (Turn)args[0];
                TurnDto turnDto = new TurnDto(
                    userId: turn.User.Id,
                    roundId: turn.Round.Id,
                    startDateTime: turn.StartDateTime,
                    endDateTime: turn.EndDateTime);
                return turnDto;
            });
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userWithIniciativeId, matchId: matchId);
        #endregion

        #region -- Assert --
        TurnDto expectedTurnDto = new TurnDto(
            userId: userWithIniciative.Id,
            roundId: match.ActiveRound.Id,
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

        _usersReadOnlyRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
            });
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
        int userId = Configuration.TestUserId;
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
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
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

        _turnsRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
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
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userId, matchId: matchId);
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
        int userWithIniciativeId = 1;
        int userWithoutIniciativeId = 2;
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

        _userMatchesRepository.Get(userWithoutIniciativeId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Success(result: new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: false,
                    new User(userWithoutIniciativeId),
                    new Match(id: matchId, startDateTime: DateTime.UtcNow))
                );
            });

        _userMatchesRepository.Get(userWithIniciativeId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Success(result: new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    new User(userWithIniciativeId),
                    new Match(id: matchId, startDateTime: DateTime.UtcNow))
                );
            });

        _userMatchesRepository.GetMany(matchId).Returns(
            (args) =>
            {
                UserMatch[] userMatches = new UserMatch[2];
                userMatches[0] = _userMatchesRepository.Get(userWithoutIniciativeId, matchId).Result;
                userMatches[1] = _userMatchesRepository.Get(userWithIniciativeId, matchId).Result;

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

        _turnsRepository.Get(Arg.Any<int>(), roundId).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<Turn>.Failure(errorMessage: $"Turn not found with userId: {userId} and roundId: {roundId}");
            });
        #endregion

        #region -- Act --
        Operation<TurnDto> useCaseOperation = _useCase.Execute(userId: userWithoutIniciativeId, matchId: matchId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn can not be created for user with id {userWithoutIniciativeId} " +
                $"in round with id {roundId} in match with id {matchId} " +
                $"since user with id {userWithIniciativeId} has not finished his turn yet",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}
