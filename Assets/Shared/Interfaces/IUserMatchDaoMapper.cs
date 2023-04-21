using System.Collections.Generic;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchDaoMapper
    {
        public UserMatchDaoJson ToDAO(UserMatch userMatch);

        public UserMatch FromDAO(UserMatchDaoJson userMatchDAO);

        public List<UserMatchDaoJson> ToDAOs(List<UserMatch> userMatches);

        public List<UserMatch> FromDAOs(List<UserMatchDaoJson> userMatchesDAOs);
    }
}
