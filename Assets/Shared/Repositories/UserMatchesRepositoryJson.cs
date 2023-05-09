using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.Shared.Utils;
using UnityEngine;

namespace TopicTwister.Shared.Repositories
{
    public class UserMatchesRepositoryJson : IUserMatchesRepository
    {
        private string _path;
        private List<UserMatchDaoJson> _writeCache;
        private List<UserMatchDaoJson> _readCache;
        private IdaoMapper<UserMatch, UserMatchDaoJson> _userMatchDaoMapper;
        

        public UserMatchesRepositoryJson(
            string resourceName,
            IdaoMapper<UserMatch, UserMatchDaoJson> userMatchDaoMapper)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            _userMatchDaoMapper = userMatchDaoMapper;
            _readCache = _userMatchDaoMapper.ToDAOs(GetAll().Result);
        }

        public Operation<UserMatch> Insert(UserMatch userMatch)
        {
            Operation<List<UserMatch>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<UserMatch>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _userMatchDaoMapper.ToDAOs(GetAllOperationResult.Result);
            _writeCache = _readCache.ToList();
            UserMatchDaoJson userMatchDaoJson = _userMatchDaoMapper.ToDAO(userMatch);
            _writeCache.Add(userMatchDaoJson);
            UserMatchDaosCollection collection = new UserMatchDaosCollection(_writeCache.ToArray());
            string data = new UserMatchDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Operation<UserMatch> getOperationResult = Get(userId: userMatch.User.Id, matchId: userMatch.Match.Id);

            return getOperationResult.WasOk ?
                getOperationResult :
                Operation<UserMatch>.Failure(errorMessage: "failure to save UserMatch");
        }

        public Operation<UserMatch> Get(int userId, int matchId)
        {
            Operation<List<UserMatch>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<UserMatch>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _userMatchDaoMapper.ToDAOs(GetAllOperationResult.Result);
            UserMatchDaoJson userMatchObtained = _readCache.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            if (userMatchObtained == null)
            {
                return Operation<UserMatch>.Failure(
                    errorMessage: $"UserMatch not found with userId: {userId} & matchId: {matchId}");
            }
            UserMatch userMatch = _userMatchDaoMapper.FromDAO(userMatchObtained);
            return Operation<UserMatch>.Success(result: userMatch);
        }

        public Operation<List<UserMatch>> GetAll()
        {
            string data = File.ReadAllText(_path);
            _readCache = new UserMatchDaosCollectionDeserializer().Deserialize(data).UserMatches;
            List<UserMatch> userMatches = _userMatchDaoMapper.FromDAOs(_readCache.ToList());
            return Operation<List<UserMatch>>.Success(result: userMatches);
        }

        public Operation<UserMatch[]> GetMany(int matchId)
        {
            Operation<List<UserMatch>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<UserMatch[]>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _userMatchDaoMapper.ToDAOs(GetAllOperationResult.Result);

            UserMatch[] userMatches = _readCache
                .Where(userMatch => userMatch.MatchId == matchId)
                .Distinct()
                .Select(_userMatchDaoMapper.FromDAO)
                .ToArray();

            if(userMatches.Length > 2)
            {
                return Operation<UserMatch[]>.Failure(errorMessage: $"Too many UserMatch instances for match with id {matchId}");
            }

            return Operation<UserMatch[]>.Success(result: userMatches);
        }
    }
}
