using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class UserMatchJsonDaoMapper : IdaoMapper<UserMatch, UserMatchDaoJson>
    {
        private IMatchesRepository _matchesRepository;
        private IUserReadOnlyRepository _userReadOnlyRepository;

        public UserMatchJsonDaoMapper(IMatchesRepository matchesRepository, IUserReadOnlyRepository userReadOnlyRepository)
        {
            _matchesRepository = matchesRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public UserMatch FromDAO(UserMatchDaoJson userMatchDAO)
        {
            return new UserMatch(
                score: userMatchDAO.Score,
                isWinner: userMatchDAO.IsWinner,
                hasInitiative: userMatchDAO.HasInitiative,
                user: _userReadOnlyRepository.Get(userMatchDAO.UserId).Outcome,
                match: _matchesRepository.Get(id: userMatchDAO.MatchId).Outcome
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
                userId: userMatch.User.Id,
                matchId: userMatch.Match.Id
                );
        }

        public List<UserMatchDaoJson> ToDAOs(List<UserMatch> userMatches)
        {
            return userMatches.Select(ToDAO).ToList();
        }
    }
}
