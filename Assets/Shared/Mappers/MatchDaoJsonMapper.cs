using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class MatchDaoJsonMapper : IMatchDaoMapper
    {
        public Match FromDAO(MatchDaoJson matchDAO)
        {
            return new Match(
                id: matchDAO.Id,
                startDateTime: matchDAO.StartDateTime,
                endDateTime: matchDAO.EndDateTime);
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
                endDateTime: match.EndDateTime);
        }

        public List<MatchDaoJson> ToDAOs(List<Match> matches)
        {
            return matches.Select(ToDAO).ToList();
        }
    }
}