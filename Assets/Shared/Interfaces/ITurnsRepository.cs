using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public interface ITurnsRepository: ITurnsReadOnlyRepository
    {
        Operation<Turn> Insert(Turn turn);
        Operation<Turn> Update(Turn turn);
    }
}
