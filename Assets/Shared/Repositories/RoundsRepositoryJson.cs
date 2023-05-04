using TopicTwister.Shared.Models;
using TopicTwister.Shared.Utils;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using System.IO;
using TopicTwister.Shared.Serialization.Serializers;


namespace TopicTwister.Shared.Repositories
{
    public class RoundsRepositoryJson : RoundsReadOnlyRepositoryJson, IRoundsRepository
    {
        private List<RoundDaoJson> _writeCache;
        private IUniqueIdGenerator _roundsIdGenerator;

        public RoundsRepositoryJson(
            string resourceName,
            IUniqueIdGenerator roundsIdGenerator,
            IMatchesReadOnlyRepository matchesReadOnlyRepository,
            ICategoriesReadOnlyRepository categoriesReadOnlyRepository) :
            base(
                resourceName,
                matchesReadOnlyRepository,
                categoriesReadOnlyRepository)
        {
            _roundsIdGenerator = roundsIdGenerator;
        }

        public Operation<Round> Insert(Round round)
        {
            Operation<List<Round>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Round>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            _writeCache = _readCache.ToList();
            RoundDaoJson roundDao = _mapper.ToDAO(round);

            roundDao = new RoundDaoJson(
                id: _roundsIdGenerator.GetNextId(),
                roundNumber: roundDao.RoundNumber,
                initialLetter: roundDao.InitialLetter,
                isActive: roundDao.IsActive,
                matchId: roundDao.MatchId,
                categoryIds: roundDao.CategoryIds);

            _writeCache.Add(roundDao);
            RoundDaosCollection collection = new RoundDaosCollection(_writeCache.ToArray());
            string data = new RoundDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Operation<Round> getOperation = Get(roundDao.Id);
            return getOperation.WasOk ? getOperation : Operation<Round>.Failure(errorMessage: "failure to insert Round");
        }

        public Operation<Round> Update(Round round)
        {
            Operation<List<Round>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Round>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _mapper.ToDAOs(GetAllOperationResult.Outcome);
            _writeCache = _readCache.ToList();
            RoundDaoJson roundDao = _mapper.ToDAO(round);
            int indexOfObjectToUpdate = _writeCache.FindIndex(dao => dao.Id == round.Id);
            _writeCache.RemoveAt(indexOfObjectToUpdate);

            roundDao = new RoundDaoJson(
                id: roundDao.Id,
                roundNumber: roundDao.RoundNumber,
                initialLetter: roundDao.InitialLetter,
                isActive: roundDao.IsActive,
                matchId: roundDao.MatchId,
                categoryIds: roundDao.CategoryIds);

            _writeCache.Add(roundDao);
            RoundDaosCollection collection = new RoundDaosCollection(_writeCache.ToArray());
            string data = new RoundDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            Operation<Round> getOperation = Get(roundDao.Id);
            return getOperation.WasOk ? getOperation : Operation<Round>.Failure(errorMessage: "failure to update Round");
        }
    }
}
