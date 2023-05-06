using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public interface ITurnsRepository
    {
        Operation<Turn> Insert(Turn turn);
    }
}
