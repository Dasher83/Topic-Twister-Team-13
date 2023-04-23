using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using TopicTwister.Home.Tests.Utils;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Repositories;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.Models;

public class UserMatchesRepositoryTests
{

    private IUserMatchesRepository _userMatchesRepository;
    private IMatchesRepository _matchRepository;
    private IUniqueIdGenerator _idGenerator;
    private IUserRepository _userRepository;

    [SetUp]
    public void SetUp()
    {
        _userRepository = new UserRepositoryInMemory();
        _idGenerator = new MatchesIdGenerator(new MatchesReadOnlyRepositoryJson(matchesResourceName: "TestData/MatchesTest"));
        _matchRepository = new MatchesRepositoryJson(matchesResourceName: "TestData/MatchesTest",idGenerator: _idGenerator);
        _userMatchesRepository = new UserMatchesRepositoryJson(
            userMatchesResourceName: "TestData/UserMatchesTest",
            matchesRepository: _matchRepository,
            userRepository: _userRepository);
    }

    [TearDown]
    public void TearDown()
    {
        new UserMatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_all_operations()
    {
        Match match = new Match();
        match = _matchRepository.Persist(match);

        List<UserMatch> userMatches = new List<UserMatch>() {
            new UserMatch(score:1,isWinner: true,hasInitiative: true,user: new User(1),match: match),
            new UserMatch(score:2,isWinner:false,hasInitiative:false,user: new User(2),match: match)
        };

        for (int i = 0; i < userMatches.Count; i++)
        {
            userMatches[i] = _userMatchesRepository.Persist(userMatches[i]);
            UserMatch expectedUserMatch = new UserMatch(score: 1, isWinner: true, hasInitiative: true, user: new User(1), match: match);
            Assert.AreEqual(expected: expectedUserMatch, actual: userMatches[i]);
        }

        for (int i = 0; i < userMatches.Count; i++)
        {
            UserMatch actualUserMatch = _userMatchesRepository.Get(userId: userMatches[i].User.Id, matchId: userMatches[i].Match.Id);
            UserMatch expectedUserMatch = userMatches[i];
            Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        }

        List<UserMatch> expectedUserMatches = _userMatchesRepository.GetAll();
        Assert.AreEqual(expected: expectedUserMatches, actual: userMatches);
    }
}
