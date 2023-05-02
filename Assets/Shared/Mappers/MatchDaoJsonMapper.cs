using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class MatchDaoJsonMapper : IdaoMapper<Match, MatchDaoJson>
    {
        IRoundsReadOnlyRepository _roundRepository;

        public MatchDaoJsonMapper(IRoundsReadOnlyRepository roundRepository)
        {
            this._roundRepository = roundRepository;
        }

        public Match FromDAO(MatchDaoJson matchDao)
        {
            List<Round> rounds = _roundRepository.GetMany(matchDao.RoundIds).Outcome;

            return new Match(
                id: matchDao.Id,
                startDateTime: matchDao.StartDateTime,
                endDateTime: matchDao.EndDateTime,
                rounds: rounds);
        }

        public List<Match> FromDAOs(List<MatchDaoJson> matchesDAOs)
        {
            return matchesDAOs.Select(FromDAO).ToList();
        }

        public MatchDaoJson ToDAO(Match match)
        {
            return new MatchDaoJson(
                id: match.Id,
                startDateTime: match.StartDateTime,
                endDateTime: match.EndDateTime,
                roundIds: match.Rounds.Select(round => round.Id).ToList());
        }

        public List<MatchDaoJson> ToDAOs(List<Match> matches)
        {
            return matches.Select(ToDAO).ToList();
        }
    }
}