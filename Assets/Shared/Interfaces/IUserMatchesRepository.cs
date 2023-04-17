using System.Collections.Generic;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserMatchesRepository
    {
        UserMatchDTO Create(int userId, int matchId, bool hasInitiative);
        UserMatchDTO Get(int userId, int matchId);
        List<UserMatchDTO> GetAll();
    }
}
