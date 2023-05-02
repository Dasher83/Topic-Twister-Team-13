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
    private IUniqueIdGenerator _idGenerator;
    private IUserRepository _userRepository;
    private IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;

    [SetUp]
    public void SetUp()
    {
        _matchDaoMapper = new MatchDaoJsonMapper();

        _matchesReadOnlyRepository = new MatchesReadOnlyRepositoryJson(
            resourceName: "TestData/Matches", matchDaoMapper: _matchDaoMapper);

        _userRepository = new UsersRepositoryInMemory();
        _idGenerator = new MatchesIdGenerator(matchesRepository: _matchesReadOnlyRepository);

        _matchesRepository = new MatchesRepositoryJson(
            matchesResourceName: "TestData/Matches",
            idGenerator: _idGenerator,
            matchDaoMapper: _matchDaoMapper);

        _userMatchesRepository = new UserMatchesRepositoryJson(
            userMatchesResourceName: "TestData/UserMatches",
            matchesRepository: _matchesRepository,
            userRepository: _userRepository);
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
        match = _matchesRepository.Save(match).Outcome;

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
            userMatches[i] = _userMatchesRepository.Save(userMatches[i]).Outcome;

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
                matchId: userMatches[i].Match.Id).Outcome;
            UserMatch expectedUserMatch = userMatches[i];
            Assert.AreEqual(expected: expectedUserMatch, actual: actualUserMatch);
        }

        List<UserMatch> expectedUserMatches = _userMatchesRepository.GetAll().Outcome;
        Assert.AreEqual(expected: expectedUserMatches, actual: userMatches);
    }
}
