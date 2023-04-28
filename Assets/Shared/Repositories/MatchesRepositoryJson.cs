using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
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
        private string _path;
        private List<MatchDaoJson> _matchesWriteCache;
        private IUniqueIdGenerator _idGenerator;
        private IdaoMapper<Match, MatchDaoJson> _mapper;

        public MatchesRepositoryJson(string matchesResourceName, IUniqueIdGenerator idGenerator): base(matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _mapper = new MatchDaoJsonMapper();
            _idGenerator = idGenerator;
        }

        public Result<Match> Save(Match match)
        {
            Result<List<Match>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Result<Match>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _matchesReadCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            _matchesWriteCache = _matchesReadCache.ToList();
            MatchDaoJson matchDAO = _mapper.ToDAO(match);

            matchDAO = new MatchDaoJson(id: _idGenerator.GetNextId(),
                startDateTime: matchDAO.StartDateTime,
                endDateTime: matchDAO.EndDateTime);

            _matchesWriteCache.Add(matchDAO);
            MatchDaosCollection collection = new MatchDaosCollection(_matchesWriteCache.ToArray());
            string data = new MatchDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Result<Match> getOperation = Get(matchDAO.Id);
            return getOperation.WasOk ? getOperation : Result<Match>.Failure(errorMessage: "failure to save Match");
        }

        public Result<bool> Delete(int id)
        {
            Result<Match> GetOperationResult = Get(id);
            if (GetOperationResult.WasOk == false)
            {
                return Result<bool>.Failure(errorMessage: GetOperationResult.ErrorMessage);
            }

            MatchDaoJson matchToDelete = _mapper.ToDAO(GetOperationResult.Outcome);
            _matchesWriteCache = _matchesReadCache.ToList();
            _matchesWriteCache.Remove(matchToDelete);
            MatchDaosCollection collection = new MatchDaosCollection(_matchesWriteCache.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);

            return Result<bool>.Success(outcome: true);
        }
    }
}
