using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserRoundReadOnlyRepository
    {
        Operation<UserRound> GetAll();
        Operation<UserRound> Get(int id);
    }
}
