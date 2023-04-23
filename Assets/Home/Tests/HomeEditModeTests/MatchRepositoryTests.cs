using System;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;


public class MatchRepositoryTests
{
    private IUniqueIdGenerator idGenerator;
    private IMatchesRepository _matchesRepository;

    [TearDown]
    public void TearDown()
    {
        new MatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_persist_match()
    {
        #region -- Arrange --
        idGenerator = Substitute.For<IUniqueIdGenerator>();
        idGenerator.GetNextId().Returns(
            (args) =>
            {
                return 3;
            });

        _matchesRepository = new MatchesRepositoryJson(
            matchesResourceName: "TestData/MatchesTest",
            idGenerator: idGenerator);

        Match match = new Match();
        #endregion

        #region -- Act --
        Match actualResult = _matchesRepository.Persist(match);
        #endregion

        #region -- Assert --
        Match expectedMatch = new Match(id: 3, startDateTime: DateTime.UtcNow, endDateTime: null);
        Assert.AreEqual(expectedMatch, actualResult);
        #endregion
    }
}
