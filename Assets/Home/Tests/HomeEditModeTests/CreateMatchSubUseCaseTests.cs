using NSubstitute;
using NSubstitute.Routing.Handlers;
using NUnit.Framework;
using System;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.Utils;


public class CreateMatchSubUseCaseTests
{
    private ICreateMatchSubUseCase _useCase;
    private IUserMatchesRepository _userMatchesRepository;
    private IdtoMapper<Match, MatchDto> _mapper;
    private IMatchesRepository _matchesRepository;
    private IUserReadOnlyRepository _userRepository;

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
        new RoundsDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_creation_of_match_and_usermatches()
    {
        #region -- Arrange --
        int testUserId = 1;
        int botId = 2;

        _mapper = Substitute.For<IdtoMapper<Match, MatchDto>>();
        _mapper.ToDTO(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match match = (Match)args[0];
                return new MatchDto(
                    id: match.Id,
                    startDateTime: match.StartDateTime,
                    endDateTime: match.EndDateTime);
            });
        _mapper.FromDTO(Arg.Any<MatchDto>()).Returns(
            (args) =>
            {
                MatchDto match = (MatchDto)args[0];
                return new Match(
                    id: match.Id,
                    startDateTime: match.StartDateTime,
                    endDateTime: match.EndDateTime);
            });

        _userRepository = Substitute.For<IUserReadOnlyRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(outcome: new User(id: userId));
            });

        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match match = (Match)args[0];
                return Operation<Match>.Success(
                    outcome: new Match(
                        id: 1,
                        startDateTime: match.StartDateTime,
                        endDateTime: match.EndDateTime));
            });

        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _userMatchesRepository.Save(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch userMatch = (UserMatch)args[0];
                return Operation<UserMatch>.Success(
                    outcome: new UserMatch(
                        score: userMatch.Score,
                        isWinner: userMatch.IsWinner,
                        hasInitiative: userMatch.HasInitiative,
                        user: new User(userMatch.User.Id),
                        match: userMatch.Match));
            });
        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int matchId = (int)args[1];

                return Operation<UserMatch>.Success(
                    outcome: new UserMatch(
                        score: 0,
                        isWinner: false,
                        hasInitiative: userId == testUserId,
                        user: new User(userId),
                        match: new Match(
                            id: matchId,
                            startDateTime: DateTime.UtcNow,
                            endDateTime: null)));
            });

        _useCase = new CreateMatchSubUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            matchDtoMapper: _mapper);
        #endregion

        #region -- Act --
        Operation<MatchDto> actualResult = _useCase.Create(testUserId, botId);
        #endregion

        #region -- Assert --
        MatchDto expectedMatch = new MatchDto(id: actualResult.Outcome.Id, startDateTime: DateTime.UtcNow, endDateTime: null);
        Assert.AreEqual(expectedMatch, actualResult.Outcome);

        UserMatch expectedUserMatch = new UserMatch(
            score: 0, isWinner: false, hasInitiative: true, user: _userRepository.Get(testUserId).Outcome, match: _mapper.FromDTO(expectedMatch));
        UserMatch actualUserMatch = _userMatchesRepository.Get(userId: testUserId, matchId: expectedMatch.Id).Outcome;
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);

        expectedUserMatch = new UserMatch(
            score: 0, isWinner: false, hasInitiative: false, user: _userRepository.Get(botId).Outcome, match: _mapper.FromDTO(expectedMatch));
        actualUserMatch = _userMatchesRepository.Get(userId: botId, matchId: expectedMatch.Id).Outcome;
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;

        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(
            (args) =>
            {
                return Operation<Match>.Success(outcome: (Match)(args[0]));
            });
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _mapper = Substitute.For<IdtoMapper<Match, MatchDto>>();

        _userRepository = Substitute.For<IUserReadOnlyRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                if(userId < 0)
                {
                    return Operation<User>.Failure(errorMessage: $"User not found with id: {userId}");
                }
                return Operation<User>.Success(new User(id: userId));
            });

        _useCase = new CreateMatchSubUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            matchDtoMapper: _mapper);
        #endregion

        #region -- Act --
        Operation<MatchDto> useCaseOperation = _useCase.Create(userId, userId);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(expected: $"User not found with id: {userId}", actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_match_not_saved()
    {
        #region -- Arrange --
        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(
            (args) =>
            {
                return Operation<Match>.Failure(errorMessage: "failure to save Match");
            });

        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _userRepository = Substitute.For<IUserReadOnlyRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(new User(id: userId));
            });

        _mapper = Substitute.For<IdtoMapper<Match, MatchDto>>();

        _useCase = new CreateMatchSubUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            matchDtoMapper: _mapper);
        #endregion

        #region -- Act --
        Operation<MatchDto> useCaseOperation = _useCase.Create(0, 1);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(expected: "failure to save Match", actual: useCaseOperation.ErrorMessage);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_userMatch_not_saved()
    {
        #region -- Arrange --
        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(
            (args) =>
                Operation<Match>.Success(
                    outcome: new Match(
                        id: 1,
                        startDateTime: DateTime.UtcNow,
                        endDateTime: null )));

        _userRepository = Substitute.For<IUserReadOnlyRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return Operation<User>.Success(outcome: new User(userId));
            });


        _mapper = Substitute.For<IdtoMapper<Match, MatchDto>>();
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _userMatchesRepository.Save(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                return Operation<UserMatch>.Failure(errorMessage: "failure to save UserMatch");
            });

        _useCase = new CreateMatchSubUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            matchDtoMapper: _mapper);
        #endregion

        #region -- Act --
        Operation<MatchDto> useCaseOperation = _useCase.Create(0, 1);
        #endregion

        #region -- Assert --
        Assert.IsFalse(useCaseOperation.WasOk);
        Assert.AreEqual(expected: "failure to save UserMatch", actual: useCaseOperation.ErrorMessage);
        #endregion
    }
}