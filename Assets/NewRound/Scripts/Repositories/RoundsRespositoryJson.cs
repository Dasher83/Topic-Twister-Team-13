using TopicTwister.Shared.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Utils;
using System.Collections.Generic;

namespace TopicTwister.NewRound.Repositories
{
    public class RoundsRespositoryJson : IRoundsRepository
    {
        public Result<List<Round>> GetMany(Match match)
        {
            throw new System.NotImplementedException();
        }

        public Result<Round> Save(Round round)
        {
            throw new System.NotImplementedException();
        }
    }
}
