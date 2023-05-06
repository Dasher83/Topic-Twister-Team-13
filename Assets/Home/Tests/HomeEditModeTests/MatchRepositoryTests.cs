using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.Utils;


public class MatchRepositoryTests
{
    private IUniqueIdGenerator idGenerator;
    private IMatchesRepository _matchesRepository;
    private IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;

    [SetUp]
    public void SetUp()
    {
        _matchDaoMapper = new MatchDaoJsonMapper();
    }

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
            resourceName: "TestData/Matches",
            matchesIdGenerator: idGenerator,
            matchDaoMapper: _matchDaoMapper);

        List<Match> matches = new List<Match>() { new Match(), new Match()};

        for(int i = 0; i < matches.Count; i++)
        {
            matches[i] = _matchesRepository.Insert(matches[i]).Outcome;
            Match expectedMatch = new Match(id: ids[i], startDateTime: DateTime.UtcNow, endDateTime: null);
            Assert.AreEqual(expected: expectedMatch, actual: matches[i]);
        }

        for (int i = 0; i < ids.Length; i++)
        {
            Match actualMatch = _matchesRepository.Get(ids[i]).Outcome;
            Match expectedMatch = matches[i];
            Assert.AreEqual(expected: expectedMatch, actual: actualMatch);
        }

        List<Match> actualMatches = _matchesRepository.GetAll().Outcome;
        Assert.AreEqual(expected: matches, actual: actualMatches);

        for(int i = 0; i < ids.Length; i++)
        {
            Operation<bool> deleteOperation = _matchesRepository.Delete(ids[i]);
            Assert.IsTrue(deleteOperation.WasOk);
            Assert.IsTrue(deleteOperation.Outcome);
        }

        idGenerator = Substitute.For<IUniqueIdGenerator>();
        idGenerator.GetNextId().Returns(
            (args) =>
            {
                return -1;
            });
        _matchesRepository = new MatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesIdGenerator: idGenerator,
            matchDaoMapper: _matchDaoMapper);

        Operation<Match> saveOperation = _matchesRepository.Insert(
                match: new Match(
                    id: -1,
                    startDateTime: DateTime.UtcNow));
        Assert.IsFalse(saveOperation.WasOk);
    }
}
