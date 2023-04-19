using NUnit.Framework;
using System;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Repositories;


public class CreateBotMatchUseCaseTests
{
    private ICreateBotMatchUseCase _useCase;
    private IUserMatchesRepository _userMatchesRepository;

    [SetUp]
    public void Setup()
    {
        _userMatchesRepository = new UserMatchesRepositoryJson(
            userMatchesResourceName: "TestData/UserMatchesTest",
            matchesRepository: new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest")
            );
        _useCase = new CreateBotMatchUseCase(
            new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest"),
            _userMatchesRepository);
    }

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
        new UserMatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_happy_path()
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

        UserMatchDTO expectedUserMatch = new UserMatchDTO(
            score: 0, isWinner: false, hasInitiative: true, userId: userId, matchId: expectedMatch.Id);
        UserMatchDTO actualUserMatch = _userMatchesRepository.Get(userId: userId, matchId: expectedMatch.Id);
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);

        expectedUserMatch = new UserMatchDTO(
            score: 0, isWinner: false, hasInitiative: false, userId: botId, matchId: expectedMatch.Id);
        actualUserMatch = _userMatchesRepository.Get(userId: botId, matchId: expectedMatch.Id);
        Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        #endregion
    }
}
