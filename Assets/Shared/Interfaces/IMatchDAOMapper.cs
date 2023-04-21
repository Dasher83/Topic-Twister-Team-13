using System.Collections.Generic;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchDAOMapper
    {
        public MatchDaoJson ToDAO(Match match);

        public Match FromDAO(MatchDaoJson matchDAO);

        public List<MatchDaoJson> ToDAOs(List<Match> matches);

        public List<Match> FromDAOs(List<MatchDaoJson> matchesDAOs);
    }
}