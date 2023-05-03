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
        

        public UserMatchesRepositoryJson(
            string resourceName,
            IMatchesRepository matchesRepository,
            IUserReadOnlyRepository userReadOnlyRepository)
        {
            _mapper = new UserMatchJsonDaoMapper(matchesRepository: matchesRepository, userReadOnlyRepository: userReadOnlyRepository);
            _path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            _userMatchesReadCache = _mapper.ToDAOs(GetAll().Outcome);
        }

        public Operation<UserMatch> Save(UserMatch userMatch)
        {
            Operation<List<UserMatch>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<UserMatch>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _userMatchesReadCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            _userMatchesWriteCache = _userMatchesReadCache.ToList();
            UserMatchDaoJson userMatchDaoJson = _mapper.ToDAO(userMatch);
            _userMatchesWriteCache.Add(userMatchDaoJson);
            UserMatchDaosCollection collection = new UserMatchDaosCollection(_userMatchesWriteCache.ToArray());
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

            _userMatchesReadCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            UserMatchDaoJson userMatchObtained = _userMatchesReadCache.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            if (userMatchObtained == null)
            {
                return Operation<UserMatch>.Failure(
                    errorMessage: $"UserMatch not found with userId: {userId} & matchId: {matchId}");
            }
            UserMatch userMatch = _mapper.FromDAO(userMatchObtained);
            return Operation<UserMatch>.Success(outcome: userMatch);
        }

        public Operation<List<UserMatch>> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatchesReadCache = new UserMatchDaosCollectionDeserializer().Deserialize(data).UserMatches;
            List<UserMatch> userMatches = _mapper.FromDAOs(_userMatchesReadCache.ToList());
            return Operation<List<UserMatch>>.Success(outcome: userMatches);
        }
    }
}
