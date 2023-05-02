using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public class UsersRepositoryInMemory : IUserRepository
    {
        private List<User> _readCache;

        public UsersRepositoryInMemory()
        {
            _readCache = new List<User>()
            {
                new User(id: 1),
                new User(id: 2)
            };
        }

        public Result<User> Get(int id)
        {
            User user = _readCache.SingleOrDefault(user => user.Id == id);
            if(user == null)
            {
                return Result<User>.Failure(errorMessage: $"User not found with id: {id}");
            }
            return Result<User>.Success(outcome: user);
        }
    }
}
