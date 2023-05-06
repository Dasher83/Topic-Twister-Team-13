using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Utils;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Serialization.Deserializers;

using System.IO;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


namespace TopicTwister.Shared.Repositories
{
    public class RoundsReadOnlyRepositoryJson : IRoundsReadOnlyRepository
    {
        protected string _path;
        protected List<RoundDaoJson> _readCache;
        protected IdaoMapper<Round, RoundDaoJson> _mapper;

        public RoundsReadOnlyRepositoryJson(
            string resourceName,
            IMatchesReadOnlyRepository matchesReadOnlyRepository,
            ICategoriesReadOnlyRepository categoriesReadOnlyRepository)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            _mapper = new RoundDaoJsonMapper(
                matchesRepository: matchesReadOnlyRepository,
                categoriesRepository: categoriesReadOnlyRepository);
            _readCache = _mapper.ToDAOs(GetAll().Result);
        }

        public Operation<List<Round>> GetAll()
        {
            string data = File.ReadAllText(_path);
            _readCache = new RoundDaosCollectionDeserializer().Deserialize(data).RoundDaos;
            List<Round> rounds = _mapper.FromDAOs(_readCache.ToList());
            return Operation<List<Round>>.Success(result: rounds);
        }

        public Operation<Round> Get(int id)
        {
            Operation<List<Round>> GetAllOperationResult = GetAll();

            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Round>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Result);
            RoundDaoJson roundDao = _readCache.SingleOrDefault(round => round.Id == id && round.Id >= 0);

            if (roundDao == null)
            {
                return Operation<Round>.Failure(errorMessage: $"Round not found with id: {id}");

            }
            Round round = _mapper.FromDAO(roundDao);

            return Operation<Round>.Success(result: round);
        }

        public Operation<List<Round>> GetMany(List<int> roundIds)
        {
            Operation<List<Round>> GetAllOperationResult = GetAll();

            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<List<Round>>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Result);

            List<Round> filteredRounds = _readCache
                .Where(roundDao => roundIds.Contains(roundDao.Id))
                .Distinct()
                .Select(roundDao => _mapper.FromDAO(roundDao))
                .ToList();

            List<int> idsFound = filteredRounds.Select(round => round.Id).ToList();
            List<int> notFoundIds = roundIds.Intersect(second: idsFound).ToList();

            if (filteredRounds.Count != roundIds.Count)
            {
                Operation<List<Round>>.Failure(errorMessage: $"Rounds not found with ids: [{string.Join(", ", notFoundIds)}]");
            }

            return Operation<List<Round>>.Success(result: filteredRounds);
        }

        public Operation<List<Round>> GetMany(int matchId)
        {
            Operation<List<Round>> GetAllOperationResult = GetAll();

            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<List<Round>>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Result);

            List<Round> filteredRounds = _readCache
                .Where(roundDao => matchId == roundDao.MatchId)
                .Distinct()
                .Select(roundDao => _mapper.FromDAO(roundDao))
                .ToList();

            if (filteredRounds.Count > 3)
            {
                Operation<List<Round>>.Failure(errorMessage: $"Too many rounds found for match with id: {matchId}");
            }

            return Operation<List<Round>>.Success(result: filteredRounds);
        }
    }
}
