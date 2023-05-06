using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Mappers
{
    public class UserMatchDaoJsonMapper : IdaoMapper<UserMatch, UserMatchDaoJson>
    {
        private IMatchesReadOnlyRepository _matchesReadOnlyRepository;
        private IUsersReadOnlyRepository _userReadOnlyRepository;

        public UserMatchDaoJsonMapper(
            IMatchesReadOnlyRepository matchesReadOnlyRepository,
            IUsersReadOnlyRepository userReadOnlyRepository)
        {
            _matchesReadOnlyRepository = matchesReadOnlyRepository;
            _userReadOnlyRepository = userReadOnlyRepository;
        }

        public UserMatch FromDAO(UserMatchDaoJson userMatchDAO)
        {
            User user = _userReadOnlyRepository.Get(id: userMatchDAO.UserId).Outcome;
            Operation<Match> getMatchOperation = _matchesReadOnlyRepository.Get(id: userMatchDAO.MatchId);
            Match match = getMatchOperation.Outcome;

            return new UserMatch(
                score: userMatchDAO.Score,
                isWinner: userMatchDAO.IsWinner,
                hasInitiative: userMatchDAO.HasInitiative,
                user: user,
                match: match);
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
                matchId: userMatch.Match.Id);
        }

        public List<UserMatchDaoJson> ToDAOs(List<UserMatch> userMatches)
        {
            return userMatches.Select(ToDAO).ToList();
        }
    }
}
