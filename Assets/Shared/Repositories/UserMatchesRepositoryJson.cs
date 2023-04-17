using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class UserMatchesRepositoryJson : IUserMatchesRepository
    {
        private string _path;
        private List<UserMatchDTO> _userMatches;
        private List<UserMatchDTO> _userMatchesToWriteCache;

        public UserMatchesRepositoryJson(string userMatchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{userMatchesResourceName}.json";
            _userMatches = GetAll();
        }

        public UserMatchDTO Create(int userId, int matchId, bool hasInitiative)
        {
            _userMatches = GetAll();
            UserMatchDTO userMatch = new UserMatchDTO(
                score: 0, isWinner: false, hasInitiative: hasInitiative,
                userId: userId, matchId: matchId);
            _userMatchesToWriteCache = _userMatches.ToList();
            _userMatchesToWriteCache.Add(userMatch);
            UserMatchesCollection collection = new UserMatchesCollection(_userMatchesToWriteCache.ToArray());
            string data = new UserMatchesCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            UserMatchDTO newUserMatch = Get(userId, matchId);
            return newUserMatch;
        }

        public UserMatchDTO Get(int userId, int matchId)
        {
            _userMatches = GetAll();
            UserMatchDTO userMatchObtained = _userMatches.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            return userMatchObtained;
        }

        public List<UserMatchDTO> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatches = new UserMatchesCollectionDeserializer().Deserialize(data).UserMatches;
            return _userMatches.ToList();
        }
    }
}
