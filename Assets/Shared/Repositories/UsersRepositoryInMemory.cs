using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;


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

        public User Get(int id)
        {
            User user = _readCache.SingleOrDefault(user => user.Id == id);
            if(user == null)
            {
                throw new UserNotFoundByRepositoryException();
            }
            return user;
        }
    }
}
