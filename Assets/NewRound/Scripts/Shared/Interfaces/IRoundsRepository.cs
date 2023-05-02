using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Shared.Interfaces
{
    public interface IRoundsRepository: IRoundsReadOnlyRepository
    {
        Result<Round> Save(Round round);
    }
}
