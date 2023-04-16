using NUnit.Framework;
using System;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Home.UseCases;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Repositories;


public class CreateBotMatchUseCaseTests
{
    private ICreateBotMatchUseCase _useCase;

    [SetUp]
    public void Setup()
    {
        _useCase = new CreateBotMatchUseCase(new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest"));
    }

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_happy_path()
    {
        #region -- Arrange --
        int idUser = 1;
        #endregion

        #region -- Act --
        MatchDTO actualResult = _useCase.Create(idUser);
        #endregion

        #region -- Assert --
        MatchDTO expectedResult = new MatchDTO(id: actualResult.Id, startDateTime: DateTime.UtcNow, endDateTime: null);
        Assert.AreEqual(expectedResult, actualResult);
        #endregion
    }
}
