using NSubstitute;
using NUnit.Framework;
using System;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.Exceptions;


public class CreateBotMatchUseCaseTests
{
    private ICreateBotMatchUseCase _useCase;
    private IUserMatchesRepository _userMatchesRepository;
    private IdtoMapper<Match, MatchDTO> _mapper;
    private IMatchesRepository _matchesRepository;
    private IUserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        _userRepository = new UserRepositoryInMemory();
        _matchesRepository = new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest");
        _userMatchesRepository = new UserMatchesRepositoryJson(
            userMatchesResourceName: "TestData/UserMatchesTest",
            matchesRepository: _matchesRepository
            );
        _mapper = new MatchDtoMapper();
        _useCase = new CreateBotMatchUseCase(
            matchesRepository: _matchesRepository,
            userMatchesRepository: _userMatchesRepository,
            userRespository: _userRepository,
            mapper: _mapper);
    }

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_creation_and_persistance_of_match_and_usermatches()
    {
        #region -- Arrange --
        int userId = 1;
        int botId = 2;
        #endregion

        #region -- Act --
        MatchDTO actualResult = _useCase.Create(userId);
        #endregion

        #region -- Assert --
        MatchDTO expectedMatch = new MatchDTO(id: actualResult.Id, startDateTime: DateTime.UtcNow, endDateTime: null);
        Assert.AreEqual(expectedMatch, actualResult);

        UserMatch expectedUserMatch = new UserMatch(
            score: 0, isWinner: false, hasInitiative: true, userId: userId, match: _mapper.FromDTO(expectedMatch));
        UserMatch actualUserMatch = _userMatchesRepository.Get(userId: userId, matchId: expectedMatch.Id);
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);

        expectedUserMatch = new UserMatch(
            score: 0, isWinner: false, hasInitiative: false, userId: botId, match: _mapper.FromDTO(expectedMatch));
        actualUserMatch = _userMatchesRepository.Get(userId: botId, matchId: expectedMatch.Id);
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        #endregion
    }

    [Test]
    public void Test_fail_due_to_unknown_user()
    {
        #region -- Arrange --
        int userId = -1;
        #endregion

        #region -- Act & Assert--
        Assert.Throws<UserNotFoundInUseCaseException>(() => _useCase.Create(userId));
        #endregion
    }

    [Test]
    public void Test_fail_due_to_match_not_persisted()
    {
        #region -- Arrange --
        _matchesRepository = Substitute.For<IMatchesRepository>();
        _matchesRepository.Persist(Arg.Any<Match>()).Returns(x => { throw new MatchNotPersistedByRepositoryException();});

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
}
