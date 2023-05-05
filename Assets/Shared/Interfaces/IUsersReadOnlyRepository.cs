using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUsersReadOnlyRepository
    {
        Operation<User> Get(int id);
    }
}
