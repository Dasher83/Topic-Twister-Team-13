using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.Shared.Mappers;
using UnityEngine;
using TopicTwister.Shared.DAOs;


namespace TopicTwister.Shared.Repositories
{
    public class UserMatchesRepositoryJson : IUserMatchesRepository
    {
        private string _path;
        private List<UserMatchDaoJson> _userMatchesWriteCache;
        private List<UserMatchDaoJson> _userMatchesReadCache;
        private IdaoMapper<UserMatch, UserMatchDaoJson> _mapper;
        

        public UserMatchesRepositoryJson(string userMatchesResourceName, IMatchesRepository matchesRepository, IUserRepository userRepository)
        {
            _mapper = new UserMatchJsonDaoMapper(matchesRepository: matchesRepository, userRepository: userRepository);
            _path = $"{Application.dataPath}/Resources/JSON/{userMatchesResourceName}.json";
            _userMatchesReadCache = _mapper.ToDAOs(GetAll());
        }

        public UserMatch Save(UserMatch userMatch)
        {
            _userMatchesReadCache = _mapper.ToDAOs(GetAll());
            _userMatchesWriteCache = _userMatchesReadCache.ToList();
            UserMatchDaoJson userMatchDaoJson = _mapper.ToDAO(userMatch);
            _userMatchesWriteCache.Add(userMatchDaoJson);
            UserMatchDaosCollection collection = new UserMatchDaosCollection(_userMatchesWriteCache.ToArray());
            string data = new UserMatchDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            UserMatch newUserMatch;
            try
            {
                newUserMatch = Get(userId: userMatch.User.Id, matchId: userMatch.Match.Id);
            }
            catch (UserMatchNotFoundByRepositoryException)
            {
                throw new UserMatchNotSabedByRepositoryException();
            }
            return newUserMatch;
        }

        public UserMatch Get(int userId, int matchId)
        {
            _userMatchesReadCache = _mapper.ToDAOs(GetAll());
            UserMatchDaoJson userMatchObtained = _userMatchesReadCache.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            if (userMatchObtained == null)
            {
                throw new UserMatchNotFoundByRepositoryException(message: $"userId '{userId}' matchId '{matchId}'");
            }
            UserMatch userMatch = _mapper.FromDAO(userMatchObtained);
            return userMatch;
        }

        public List<UserMatch> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatchesReadCache = new UserMatchDaosCollectionDeserializer().Deserialize(data).UserMatches;
            List<UserMatch> userMatches = _mapper.FromDAOs(_userMatchesReadCache.ToList());
            return userMatches;
        }
    }
}
