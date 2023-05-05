using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public class UsersReadOnlyRepositoryInMemory : IUsersReadOnlyRepository
    {
        private List<User> _readCache;

        public UsersReadOnlyRepositoryInMemory()
        {
            _readCache = new List<User>()
            {
                new User(id: 0),
                new User(id: 1),
                new User(id: 2)
            };
        }

        public Operation<User> Get(int id)
        {
            User user = _readCache.SingleOrDefault(user => user.Id == id);
            if(user == null)
            {
                return Operation<User>.Failure(errorMessage: $"User not found with id: {id}");
            }
            return Operation<User>.Success(outcome: user);
        }
    }
}
