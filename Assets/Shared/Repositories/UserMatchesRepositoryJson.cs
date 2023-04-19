using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.SharedMappers;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class UserMatchesRepositoryJson : IUserMatchesRepository
    {
        private string _path;
        private List<UserMatchDTO> _userMatchesWriteCache;
        private List<UserMatchDTO> _userMatchesReadCache;
        private UserMatchMapper _mapper;
        private IMatchesRepository _matchRepository;

        public UserMatchesRepositoryJson(string userMatchesResourceName, IMatchesRepository matchesRepository)
        {
            _matchRepository = matchesRepository;
            _mapper = new UserMatchMapper(matchesRepository: _matchRepository);
            _path = $"{Application.dataPath}/Resources/JSON/{userMatchesResourceName}.json";
            _userMatchesReadCache = _mapper.ToDTOs(GetAll());
        }

        public UserMatchDTO Create(int userId, int matchId, bool hasInitiative)
        {
            _userMatchesReadCache = _mapper.ToDTOs(GetAll());
            UserMatchDTO userMatch = new UserMatchDTO(
                score: 0, isWinner: false, hasInitiative: hasInitiative,
                userId: userId, matchId: matchId);
            _userMatchesWriteCache = _userMatchesReadCache.ToList();
            _userMatchesWriteCache.Add(userMatch);
            UserMatchesCollection collection = new UserMatchesCollection(_userMatchesWriteCache.ToArray());
            string data = new UserMatchesCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            UserMatchDTO newUserMatch = Get(userId, matchId);
            return newUserMatch;
        }

        public UserMatchDTO Get(int userId, int matchId)
        {
            _userMatchesReadCache = _mapper.ToDTOs(GetAll());
            UserMatchDTO userMatchObtained = _userMatchesReadCache.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            return userMatchObtained;
        }

        public List<UserMatch> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatchesReadCache = new UserMatchesCollectionDeserializer().Deserialize(data).UserMatches;
            List<UserMatch> userMatches = _mapper.FromDTOs(_userMatchesReadCache.ToList());
            return userMatches;
        }
    }
}
