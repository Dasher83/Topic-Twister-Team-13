using System;
using System.Collections.Generic;
using System.Linq;
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
    public void Test_ok_for_user_without_iniciative_inside_time_limit_end_of_round()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_inside_time_limit_end_of_match()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_outside_time_limit_end_of_round()
    {
        throw new NotImplementedException();
    }

    [Test]
    public void Test_ok_for_user_without_iniciative_outside_time_limit_end_of_match()
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
                    rounds: new List<Round>() { round });

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
                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    round: _roundsReadOnlyRepository.Get(id: roundId).Result,
                    startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds),
                    endDateTime: DateTime.UtcNow);

                return Operation<Turn>.Success(result: turn);
            });
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(userId: userId, matchId: matchId, answerDtos: new AnswerDto[Configuration.CategoriesPerRound]);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Turn already ended for user with id {userId} " +
                    $"in round with id {roundId} in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_too_many_or_too_few_answers()
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
                    rounds: new List<Round>() { round });

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

        List<Category> categories = new List<Category>();
        categories.Add(new Category(id: 0, name: ""));
        categories.Add(new Category(id: 1, name: ""));
        categories.Add(new Category(id: 2, name: ""));
        categories.Add(new Category(id: 3, name: ""));
        categories.Add(new Category(id: 4, name: ""));
        categories.Add(new Category(id: 5, name: ""));
        
        _roundsReadOnlyRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories.Take(Configuration.CategoriesPerRound).ToList());

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
                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    round: _roundsReadOnlyRepository.Get(id: roundId).Result,
                    startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds));

                return Operation<Turn>.Success(result: turn);
            });
        
        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound + 1];
        CategoryDto categoryDto;
        for (int i = 0; i < answerDtos.Length; i++)
        {
            categoryDto = new CategoryDto(id: categories[i].Id, name: categories[i].Name);
            answerDtos[i] = new AnswerDto(categoryDto: categoryDto, userInput: "", order: i);
        }
        #endregion

        #region -- Act & Assert --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(
                userId: userId,
                matchId: matchId,
                answerDtos: answerDtos.Take(Configuration.CategoriesPerRound - 1).ToArray());

        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Too few answers for turn of user with id {userId} " +
                $"for round with id {roundId} for match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);

        useCaseOperation = _useCase
            .Execute(
                userId: userId,
                matchId: matchId,
                answerDtos: answerDtos.Take(Configuration.CategoriesPerRound + 1).ToArray());

        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Too many answers for turn of user with id {userId} " +
                $"for round with id {roundId} for match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_category_mismatch()
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
                    rounds: new List<Round>() { round });

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

        List<Category> categories = new List<Category>();
        categories.Add(new Category(id: 0, name: ""));
        categories.Add(new Category(id: 1, name: ""));
        categories.Add(new Category(id: 2, name: ""));
        categories.Add(new Category(id: 3, name: ""));
        categories.Add(new Category(id: 4, name: ""));
        
        _roundsReadOnlyRepository.Get(roundId).Returns(
            (args) =>
            {
                Round round = new Round(
                    id: roundId,
                    roundNumber: 0,
                    initialLetter: 'f',
                    isActive: true,
                    match: _matchesReadOnlyRepository.Get(id: matchId).Result,
                    categories: categories);

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
                Turn turn = new Turn(
                    user: _usersReadOnlyRepository.Get(id: userId).Result,
                    round: _roundsReadOnlyRepository.Get(id: roundId).Result,
                    startDateTime: DateTime.UtcNow - TimeSpan.FromSeconds(Configuration.TurnDurationInSeconds));

                return Operation<Turn>.Success(result: turn);
            });

        AnswerDto[] answerDtos = new AnswerDto[Configuration.CategoriesPerRound];
        CategoryDto categoryDto;
        for (int i = 0; i < answerDtos.Length-1; i++)
        {
            categoryDto = new CategoryDto(id: categories[i].Id, name: categories[i].Name);
            answerDtos[i] = new AnswerDto(categoryDto: categoryDto, userInput: "", order: i);
        }
        
        categoryDto = new CategoryDto(id: -1, name: "");
        answerDtos[^1] = new AnswerDto(categoryDto: categoryDto, userInput: "", order: answerDtos.Length-1);
        #endregion

        #region -- Act --
        Operation<TurnWithEvaluatedAnswersDto> useCaseOperation = _useCase
            .Execute(
                userId: userId,
                matchId: matchId,
                answerDtos: answerDtos);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(
            expected: $"Some of your answers don't match the categories for round with id {roundId} in match with id {matchId}",
            actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}
