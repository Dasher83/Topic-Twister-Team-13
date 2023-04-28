using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;
using System;
using System.Linq;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.UseCases.Utils;


public class CreateBotMatchUseCaseTests
{
    private ICreateBotMatchUseCase _useCase;
    private IUserMatchesRepository _userMatchesRepository;
    private IdtoMapper<Match, MatchDTO> _mapper;
    private IMatchesRepository _matchesRepository;
    private IUserRepository _userRepository;

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_creation_of_match_and_usermatches()
    {
        #region -- Arrange --
        int testUserId = 1;
        int botId = 2;

        _mapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();
        _mapper.ToDTO(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match match = (Match)args[0];
                return new MatchDTO(
                    id: match.Id,
                    startDateTime: match.StartDateTime,
                    endDateTime: match.EndDateTime);
            });
        _mapper.FromDTO(Arg.Any<MatchDTO>()).Returns(
            (args) =>
            {
                MatchDTO match = (MatchDTO)args[0];
                return new Match(
                    id: match.Id,
                    startDateTime: match.StartDateTime,
                    endDateTime: match.EndDateTime);
            });

        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return new User(id: userId);
            });

        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(
            (args) =>
            {
                Match match = (Match)args[0];
                return new Match(
                    id: 1,
                    startDateTime: match.StartDateTime,
                    endDateTime: match.EndDateTime);
            });

        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _userMatchesRepository.Save(Arg.Any<UserMatch>()).Returns(
            (args) =>
            {
                UserMatch userMatch = (UserMatch)args[0];
                return new UserMatch(
                    score: userMatch.Score,
                    isWinner: userMatch.IsWinner,
                    hasInitiative: userMatch.HasInitiative,
                    user: new User(userMatch.User.Id),
                    match: userMatch.Match);
            });
        _userMatchesRepository.Get(Arg.Any<int>(), Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                int matchId = (int)args[1];

                return new UserMatch(
                    score: 0,
                    isWinner: false,
                    hasInitiative: userId == testUserId,
                    user: new User(userId),
                    match: new Match(
                        id: matchId,
                        startDateTime: DateTime.UtcNow,
                        endDateTime: null));
            });

        _useCase = new CreateBotMatchUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            mapper: _mapper);
        #endregion

        #region -- Act --
        UseCaseResult<MatchDTO> actualResult = _useCase.Create(testUserId);
        #endregion

        #region -- Assert --
        MatchDTO expectedMatch = new MatchDTO(id: actualResult.Outcome.Id, startDateTime: DateTime.UtcNow, endDateTime: null);
        Assert.AreEqual(expectedMatch, actualResult.Outcome);

        UserMatch expectedUserMatch = new UserMatch(
            score: 0, isWinner: false, hasInitiative: true, user: _userRepository.Get(testUserId), match: _mapper.FromDTO(expectedMatch));
        UserMatch actualUserMatch = _userMatchesRepository.Get(userId: testUserId, matchId: expectedMatch.Id);
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);

        expectedUserMatch = new UserMatch(
            score: 0, isWinner: false, hasInitiative: false, user: _userRepository.Get(botId), match: _mapper.FromDTO(expectedMatch));
        actualUserMatch = _userMatchesRepository.Get(userId: botId, matchId: expectedMatch.Id);
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;

        _matchesRepository = Substitute.For<IMatchesRepository>();
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _mapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();

        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                if(userId < 0)
                {
                    throw new UserNotFoundByRepositoryException();
                }
                return new User(id: userId);
            });

        _useCase = new CreateBotMatchUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            mapper: _mapper);
        #endregion

        #region -- Act & Assert --
        UserNotFoundInUseCaseException exception = Assert.Throws<UserNotFoundInUseCaseException>(() => _useCase.Create(userId));
        Assert.IsNotNull(exception);
        Assert.AreEqual(expected: $"userId: {userId}", actual: exception.Message);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_match_not_persisted()
    {
        #region -- Arrange --
        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(args => { throw new MatchNotSavedByRepositoryException();});

        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();

        _useCase = new CreateBotMatchUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            mapper: _mapper);
        #endregion

        #region -- Act & Assert--
        Assert.Throws<MatchNotCreatedInUseCaseException>(() => _useCase.Create(0));
        #endregion
    }

    [Test]
    public void Test_fail_due_to_userMatch_not_persisted()
    {
        #region -- Arrange --
        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Save(Arg.Any<Match>()).Returns(args =>  new Match(id: 1,startDateTime: DateTime.UtcNow,endDateTime: null ));

        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.Get(Arg.Any<int>()).Returns(
            (args) =>
            {
                int userId = (int)args[0];
                return new User(userId);
            });


        _mapper = Substitute.For<IdtoMapper<Match, MatchDTO>>();
        _userMatchesRepository = Substitute.For<IUserMatchesRepository>();
        _userMatchesRepository.Save(Arg.Any<UserMatch>()).Returns(args => { throw new UserMatchNotSabedByRepositoryException(); });

        _useCase = new CreateBotMatchUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            mapper: _mapper);


        #endregion

        #region -- Act & Assert--
        Assert.Throws<MatchNotCreatedInUseCaseException>(() => _useCase.Create(0));
        #endregion
    }
}
