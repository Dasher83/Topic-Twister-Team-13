using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IMatchesRepository
    {
        Match Persist(Match match);
        List<MatchDTO> GetAll();
        Match Get(int id);
    }
}
