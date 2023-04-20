using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchesRepository
    {
        UserMatchDTO Create(int userId, int matchId, bool hasInitiative);
        UserMatch Get(int userId, int matchId);
        List<UserMatch> GetAll();
    }
}
