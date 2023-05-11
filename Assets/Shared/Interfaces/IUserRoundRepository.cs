using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserRoundRepository
    {
        Operation<UserRound> Insert();
    }
}
