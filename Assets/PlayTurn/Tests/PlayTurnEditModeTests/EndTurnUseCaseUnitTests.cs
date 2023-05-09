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


public class EndTurnUseCaseUnitTests
{
    private IEndTurnUseCase _useCase;
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUserMatchesRepository _userMatchesRepository;
    private ITurnsRepository _turnsRepository;
    private IRoundsReadOnlyRepository _roundsReadOnlyRepository;

    [SetUp]
    public void SetUp()
    {
        _usersReadOnlyRepository = Substitute.For<IUsersReadOnlyRepository>();

        _matchesReadOnlyRepository = Substitute.For<IMatchesReadOnlyRepository>();

        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();

        _turnsRepository = Substitute.For<ITurnsRepository>();

        _roundsReadOnlyRepository = Substitute.For<IRoundsReadOnlyRepository>();

        _useCase = new EndTurnUseCase(
            usersReadOnlyRepository: _usersReadOnlyRepository,
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userMatchesRepository: _userMatchesRepository,
            turnsRepository: _turnsRepository,
            roundsReadOnlyRepository: _roundsReadOnlyRepository);
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

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
            });
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

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                return Operation<Match>.Failure(errorMessage: $"Match not found with id: {matchId}");
            });
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
        int matchId = 0;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                return Operation<Match>.Success(result: new Match(id: matchId, startDateTime: DateTime.UtcNow));
            });

        _userMatchesRepository.Get(userId, matchId).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Failure(
                    errorMessage: $"User with id {userId} is not involved in match with id {matchId}");
            });
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"User with id {userId} is not involved in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_turn()
    {
        #region -- Arrange --
        int userId = Configuration.TestUserId;
        int matchId = 0;
        int roundId = 0;
        Match match;
        Round round;

        _usersReadOnlyRepository.Get(userId).Returns(
            (args) =>
            {
                return Operation<User>.Success(result: new User(id: userId));
            });

        _matchesReadOnlyRepository.Get(matchId).Returns(
            (args) =>
            {
                match = new Match(
                    id: matchId,
                    startDateTime: DateTime.UtcNow);

                round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'j',
                    isActive: true,
                    match: match,
                    categories: new List<Category>());

                match = new Match(
                    id: matchId,
                    startDateTime: match.StartDateTime,
                    endDateTime: null,
                    rounds: new List<Round>() { round } );

                return Operation<Match>.Success(result: match);
            });

        _userMatchesRepository.Get(userId, matchId).Returns(
            (args) =>
            {
                UserMatch userMatch = new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: true,
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    match: _matchesReadOnlyRepository.Get(matchId).Result);

                return Operation<UserMatch>.Success(result: userMatch);
            });

        _roundsReadOnlyRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: new List<Category>());

                return Operation<Round>.Success(result: round);
            });

        _roundsReadOnlyRepository.GetMany(matchId).Returns(
            (args) =>
            {
                List<Round> rounds = new List<Round>();
                Round activeRound = _roundsReadOnlyRepository.Get(roundId).Result;
                rounds.Add(activeRound);
                return Operation<List<Round>>.Success(result: rounds);
            });

        _turnsRepository.Get(userId, roundId).Returns(
            (args) =>
            {
                string errorMessage =
                    $"Turn not found for user with id {userId} " +
                    $"in round with id {roundId} in match with id {matchId}";

                return Operation<Turn>.Failure(errorMessage: errorMessage);
            });
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn not found for user with id {userId} in round with id {roundId} in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_already_ended_turn()
    {
        throw new NotImplementedException();
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