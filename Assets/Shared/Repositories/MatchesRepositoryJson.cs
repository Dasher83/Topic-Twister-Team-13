using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesRepositoryJson : MatchesReadOnlyRepositoryJson, IMatchesRepository
    {
        private List<MatchDaoJson> _writeCache;
        private IUniqueIdGenerator _matchesIdGenerator;

        public MatchesRepositoryJson(
            string resourceName,
            IUniqueIdGenerator matchesIdGenerator,
            IdaoMapper<Match, MatchDaoJson> matchDaoMapper) : base(resourceName, matchDaoMapper)
        {
            _matchesIdGenerator = matchesIdGenerator;
        }

        public Operation<Match> Save(Match match)
        {
            Operation<List<Match>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Match>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _matchDaoMapper.ToDAOs(GetAllOperationResult.Outcome);
            _writeCache = _readCache.ToList();
            MatchDaoJson matchDao = _matchDaoMapper.ToDAO(match);

            matchDao = new MatchDaoJson(id: _matchesIdGenerator.GetNextId(),
                startDateTime: matchDao.StartDateTime,
                endDateTime: matchDao.EndDateTime);

            _writeCache.Add(matchDao);
            MatchDaosCollection collection = new MatchDaosCollection(_writeCache.ToArray());
            string data = new MatchDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Operation<Match> getOperation = Get(matchDao.Id);
            return getOperation.WasOk ? getOperation : Operation<Match>.Failure(errorMessage: "failure to save Match");
        }

        public Operation<bool> Delete(int id)
        {
            Operation<Match> GetOperationResult = Get(id);
            if (GetOperationResult.WasOk == false)
            {
                return Operation<bool>.Failure(errorMessage: GetOperationResult.ErrorMessage);
            }

            MatchDaoJson matchToDelete = _matchDaoMapper.ToDAO(GetOperationResult.Outcome);
            _writeCache = _readCache.ToList();
            _writeCache.Remove(matchToDelete);
            MatchDaosCollection collection = new MatchDaosCollection(_writeCache.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);

            return Operation<bool>.Success(outcome: true);
        }
    }
}
