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
            IMatchesReadOnlyRepository matchesRepository,
            ICategoriesReadOnlyRepository categoriesRepository)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            _mapper = new RoundDaoJsonMapper(
                matchesRepository: matchesRepository,
                categoriesRepository: categoriesRepository);
            _readCache = _mapper.ToDAOs(GetAll().Outcome);
        }

        public Result<List<Round>> GetAll()
        {
            string data = File.ReadAllText(_path);
            _readCache = new RoundDaosCollectionDeserializer().Deserialize(data).RoundDaos;
            List<Round> rounds = _mapper.FromDAOs(_readCache.ToList());
            return Result<List<Round>>.Success(outcome: rounds);
        }

        public Result<Round> Get(int id)
        {
            Result<List<Round>> GetAllOperationResult = GetAll();

            if (GetAllOperationResult.WasOk == false)
            {
                return Result<Round>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            RoundDaoJson roundDao = _readCache.SingleOrDefault(round => round.Id == id && round.Id >= 0);

            if (roundDao == null)
            {
                return Result<Round>.Failure(errorMessage: $"Round not found with id: {id}");

            }
            Round round = _mapper.FromDAO(roundDao);

            return Result<Round>.Success(outcome: round);
        }

        public Result<List<Round>> GetMany(List<int> roundIds)
        {
            Result<List<Round>> GetAllOperationResult = GetAll();

            if (GetAllOperationResult.WasOk == false)
            {
                return Result<List<Round>>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);

            List<Round> filteredRounds = _readCache
                .Where(roundDao => roundIds.Contains(roundDao.Id))
                .Distinct()
                .Select(roundDao => _mapper.FromDAO(roundDao))
                .ToList();

            List<int> idsFound = filteredRounds.Select(round => round.Id).ToList();
            List<int> notFoundIds = roundIds.Intersect(second: idsFound).ToList();

            if (filteredRounds.Count != roundIds.Count)
            {
                Result<List<Round>>.Failure(errorMessage: $"Rounds not found with ids: [{string.Join(", ", notFoundIds)}]");
            }

            return Result<List<Round>>.Success(outcome: filteredRounds);
        }

        public Result<List<Round>> GetMany(int matchId)
        {
            Result<List<Round>> GetAllOperationResult = GetAll();

            if (GetAllOperationResult.WasOk == false)
            {
                return Result<List<Round>>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);

            List<Round> filteredRounds = _readCache
                .Where(roundDao => matchId == roundDao.MatchId)
                .Distinct()
                .Select(roundDao => _mapper.FromDAO(roundDao))
                .ToList();

            if (filteredRounds.Count > 3)
            {
                Result<List<Round>>.Failure(errorMessage: $"Too many rounds found for match with id: {matchId}");
            }

            return Result<List<Round>>.Success(outcome: filteredRounds);
        }
    }
}
