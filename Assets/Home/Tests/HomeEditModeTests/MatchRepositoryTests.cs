using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.Exceptions;

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
    public void Test_ok_all_operations()
    {
        int index = 0;
        int[] ids = new int[2] { 3, 8 };
        idGenerator = Substitute.For<IUniqueIdGenerator>();
        idGenerator.GetNextId().Returns(
            (args) =>
            {
                int result = ids[index];
                index++;
                return result;
            });

        _matchesRepository = new MatchesRepositoryJson(
            matchesResourceName: "TestData/MatchesTest",
            idGenerator: idGenerator);

        List<Match> matches = new List<Match>() { new Match(), new Match()};

        for(int i = 0; i < matches.Count; i++)
        {
            matches[i] = _matchesRepository.Save(matches[i]);
            Match expectedMatch = new Match(id: ids[i], startDateTime: DateTime.UtcNow, endDateTime: null);
            Assert.AreEqual(expected: expectedMatch, actual: matches[i]);
        }

        for (int i = 0; i < ids.Length; i++)
        {
            Match actualMatch = _matchesRepository.Get(ids[i]);
            Match expectedMatch = matches[i];
            Assert.AreEqual(expected: expectedMatch, actual: actualMatch);
        }

        List<Match> expectedMatches = _matchesRepository.GetAll();
        Assert.AreEqual(expected: expectedMatches, actual: matches);

        for(int i = 0; i < ids.Length; i++)
        {
            try
            {
                _matchesRepository.Delete(ids[i]);
                Assert.Throws<MatchNotFoundByRespositoryException>(() => _matchesRepository.Get(id: ids[i]));
            }
            catch (MatchNotFoundByRespositoryException) { }
        }

        idGenerator = Substitute.For<IUniqueIdGenerator>();
        idGenerator.GetNextId().Returns(
            (args) =>
            {
                return -1;
            });
        _matchesRepository = new MatchesRepositoryJson(
            matchesResourceName: "TestData/MatchesTest",
            idGenerator: idGenerator);

        try
        {
            Assert.Throws<MatchNotSavedByRepositoryException>(
            () =>
            {
                _matchesRepository.Save(match: new Match(id: -1, startDateTime: DateTime.UtcNow, endDateTime: null));
            });
        }
        catch (MatchNotSavedByRepositoryException) { }
    }
}
