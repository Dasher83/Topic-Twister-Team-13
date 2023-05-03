using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserReadOnlyRepository
    {
        Operation<User> Get(int userId);
    }
}