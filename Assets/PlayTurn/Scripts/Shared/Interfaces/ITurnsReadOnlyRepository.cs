using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.PlayTurn.Shared.Interfaces
{
    public interface ITurnsReadOnlyRepository
    {
        Operation<Turn> Get(int userId, int roundId);
    }
}
