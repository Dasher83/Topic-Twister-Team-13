using System.Collections.Generic;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchDaoMapper
    {
        public MatchDaoJson ToDAO(Match match);

        public Match FromDAO(MatchDaoJson matchDAO);

        public List<MatchDaoJson> ToDAOs(List<Match> matches);

        public List<Match> FromDAOs(List<MatchDaoJson> matchesDAOs);
    }
}