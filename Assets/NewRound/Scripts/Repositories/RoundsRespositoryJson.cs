using TopicTwister.Shared.Models;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Utils;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using System.IO;
using TopicTwister.NewRound.Shared.Serialization.Serializers;


namespace TopicTwister.NewRound.Repositories
{
    public class RoundsRespositoryJson : RoundsReadOnlyRepositoryJson, IRoundsRepository
    {
        private List<RoundDaoJson> _writeCache;
        private IUniqueIdGenerator _idGenerator;

        public RoundsRespositoryJson(
            string resourceName,
            IUniqueIdGenerator idGenerator,
            IMatchesReadOnlyRepository matchesRepository,
            ICategoriesReadOnlyRepository categoriesRepository) : base(resourceName, matchesRepository, categoriesRepository)
        {
            _idGenerator = idGenerator;
        }

        public Result<Round> Save(Round round)
        {
            Result<List<Round>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Result<Round>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            _writeCache = _readCache.ToList();
            RoundDaoJson roundDao = _mapper.ToDAO(round);

            roundDao = new RoundDaoJson(
                id: _idGenerator.GetNextId(),
                roundNumber: roundDao.RoundNumber,
                initialLetter: roundDao.InitialLetter,
                isActive: roundDao.IsActive,
                matchId: roundDao.MatchId,
                categoryIds: roundDao.CategoryIds);

            _writeCache.Add(roundDao);
            RoundDaosCollection collection = new RoundDaosCollection(_writeCache.ToArray());
            string data = new RoundDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Result<Round> getOperation = Get(roundDao.Id);
            return getOperation.WasOk ? getOperation : Result<Round>.Failure(errorMessage: "failure to save Round");
        }
    }
}
