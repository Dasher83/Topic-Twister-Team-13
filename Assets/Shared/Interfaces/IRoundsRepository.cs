using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IRoundsRepository: IRoundsReadOnlyRepository
    {
        Operation<Round> Insert(Round round);
        Operation<Round> Update(Round round);
    }
}
