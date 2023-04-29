using TopicTwister.NewRound.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IRoundsRepository
    {
        Result<Round> Save(Round round);
    }
}
