using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.Shared.Mappers;
using UnityEngine;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Utils;

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
            _userMatchesReadCache = _mapper.ToDAOs(GetAll().Outcome);
        }

        public Result<UserMatch> Save(UserMatch userMatch)
        {
            Result<List<UserMatch>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Result<UserMatch>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _userMatchesReadCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            _userMatchesWriteCache = _userMatchesReadCache.ToList();
            UserMatchDaoJson userMatchDaoJson = _mapper.ToDAO(userMatch);
            _userMatchesWriteCache.Add(userMatchDaoJson);
            UserMatchDaosCollection collection = new UserMatchDaosCollection(_userMatchesWriteCache.ToArray());
            string data = new UserMatchDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Result<UserMatch> getOperationResult = Get(userId: userMatch.User.Id, matchId: userMatch.Match.Id);

            return getOperationResult.WasOk ?
                getOperationResult :
                Result<UserMatch>.Failure(errorMessage: "failure to save UserMatch");
        }

        public Result<UserMatch> Get(int userId, int matchId)
        {
            Result<List<UserMatch>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Result<UserMatch>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _userMatchesReadCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            UserMatchDaoJson userMatchObtained = _userMatchesReadCache.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            if (userMatchObtained == null)
            {
                return Result<UserMatch>.Failure(
                    errorMessage: $"UserMatch not found with userId: {userId} & matchId: {matchId}");
            }
            UserMatch userMatch = _mapper.FromDAO(userMatchObtained);
            return Result<UserMatch>.Success(outcome: userMatch);
        }

        public Result<List<UserMatch>> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatchesReadCache = new UserMatchDaosCollectionDeserializer().Deserialize(data).UserMatches;
            List<UserMatch> userMatches = _mapper.FromDAOs(_userMatchesReadCache.ToList());
            return Result<List<UserMatch>>.Success(outcome: userMatches);
        }
    }
}
