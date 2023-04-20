using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Interfaces
{
    public interface IUserRepository
    {
        User Get(int userId);
    }
}