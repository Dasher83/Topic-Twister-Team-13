using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class UserMatchJsonDaoMapper : IUserMatchDaoMapper
    {
        private IMatchesRepository _matchesRepository;

        public UserMatchJsonDaoMapper(IMatchesRepository matchesRepository)
        {
            _matchesRepository = matchesRepository;
        }

        public UserMatch FromDAO(UserMatchDaoJson userMatchDAO)
        {
            return new UserMatch(
                score: userMatchDAO.Score,
                isWinner: userMatchDAO.IsWinner,
                hasInitiative: userMatchDAO.HasInitiative,
                userId: userMatchDAO.UserId,
                match: _matchesRepository.Get(id: userMatchDAO.MatchId)
                );
        }

        public List<UserMatch> FromDAOs(List<UserMatchDaoJson> userMatchesDAOs)
        {
            return userMatchesDAOs.Select(FromDAO).ToList();
        }

        public UserMatchDaoJson ToDAO(UserMatch userMatch)
        {
            return new UserMatchDaoJson(
                score: userMatch.Score,
                isWinner: userMatch.IsWinner,
                hasInitiative: userMatch.HasInitiative,
                userId: userMatch.UserId,
                matchId: userMatch.Match.Id
                );
        }

        public List<UserMatchDaoJson> ToDAOs(List<UserMatch> userMatches)
        {
            return userMatches.Select(ToDAO).ToList();
        }
    }
}
