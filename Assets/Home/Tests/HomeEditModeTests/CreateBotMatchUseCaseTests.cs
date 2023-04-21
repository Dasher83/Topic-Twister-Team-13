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


public class CreateBotMatchUseCaseTests
{
    private ICreateBotMatchUseCase _useCase;
    private IUserMatchesRepository _userMatchesRepository;
    private IdtoMapper<Match, MatchDTO> _mapper;

    [SetUp]
    public void Setup()
    {
        _userMatchesRepository = new UserMatchesRepositoryJson(
            userMatchesResourceName: "TestData/UserMatchesTest",
            matchesRepository: new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest")
            );
        _mapper = new MatchDtoMapper();
        _useCase = new CreateBotMatchUseCase(
            new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest"),
            _userMatchesRepository,
            userRespository: new UserRepositoryInMemory(),
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

        //Create:
        var calculator = Substitute.For<ICalculator>();

        //Set a return value:
        calculator.Add(1, 2).Returns(3);
        Assert.AreEqual(3, calculator.Add(1, 2));

        //Check received calls:
        calculator.Received().Add(1, Arg.Any<int>());
        calculator.DidNotReceive().Add(2, 2);

        //Raise events
        calculator.PoweringUp += Raise.Event();
    }

    public interface ICalculator
    {
        int Add(int a, int b);
        event EventHandler PoweringUp;
    }
}
