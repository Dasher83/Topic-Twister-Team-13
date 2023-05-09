using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Repositories;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.TestUtils;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Mappers;


public class UserMatchesRepositoryTests
{
    private IUserMatchesRepository _userMatchesRepository;
    private IMatchesRepository _matchesRepository;
    private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
    private IUniqueIdGenerator _matchIdGenerator;
    private IUsersReadOnlyRepository _usersReadOnlyRepository;
    private IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;
    private IdaoMapper<UserMatch, UserMatchDaoJson> _userMatchDaoMapper;

    [SetUp]
    public void SetUp()
    {
        _matchDaoMapper = new MatchDaoJsonMapper();

        _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
            resourceName: "TestData/Matches", matchDaoMapper: _matchDaoMapper);

        _usersReadOnlyRepository = new UsersReadOnlyRepositoryInMemory();
        _matchIdGenerator = new MatchesIdGenerator(matchesReadOnlyRepository: _matchesReadOnlyRepository);

        _matchesRepository = new MatchesRepositoryJson(
            resourceName: "TestData/Matches",
            matchesIdGenerator: _matchIdGenerator,
            matchDaoMapper: _matchDaoMapper);

        _userMatchDaoMapper = new UserMatchDaoJsonMapper(
            matchesReadOnlyRepository: _matchesReadOnlyRepository,
            userReadOnlyRepository: _usersReadOnlyRepository);

        _userMatchesRepository = new UserMatchesRepositoryJson(
            resourceName: "TestData/UserMatches",
            userMatchDaoMapper: _userMatchDaoMapper);
    }

    [TearDown]
    public void TearDown()
    {
        new UserMatchesDeleteJson().Delete();
        new MatchesDeleteJson().Delete();
    }

    [Test]
    public void Test_ok_all_operations()
    {
        Match match = new Match();
        match = _matchesRepository.Insert(match).Result;

        List<UserMatch> userMatches = new List<UserMatch>() {
            new UserMatch(
                score:1,
                isWinner: true,
                hasInitiative: true,
                user: new User(1),
                match: match),

            new UserMatch(
                score:2,
                isWinner:false,
                hasInitiative:false,
                user: new User(2),
                match: match)
        };

        for (int i = 0; i < userMatches.Count; i++)
        {
            userMatches[i] = _userMatchesRepository.Insert(userMatches[i]).Result;

            UserMatch expectedUserMatch = new UserMatch(
                score: userMatches[i].Score,
                isWinner: userMatches[i].IsWinner,
                hasInitiative: userMatches[i].HasInitiative,
                user: userMatches[i].User,
                match: userMatches[i].Match);

            Assert.AreEqual(expected: expectedUserMatch, actual: userMatches[i]);
        }

        for (int i = 0; i < userMatches.Count; i++)
        {
            UserMatch actualUserMatch = _userMatchesRepository.Get(
                userId: userMatches[i].User.Id,
                matchId: userMatches[i].Match.Id).Result;
            UserMatch expectedUserMatch = userMatches[i];
            Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        }

        List<UserMatch> expectedUserMatches = _userMatchesRepository.GetAll().Result;
        Assert.AreEqual(expected: expectedUserMatches, actual: userMatches);
    }
}
