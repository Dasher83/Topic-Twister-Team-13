using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public class TurnsRepositoryJson : TurnsReadOnlyRepositoryJson, ITurnsRepository
    {
        private List<TurnDaoJson> _writeCache;

        public TurnsRepositoryJson(string resourceName, IdaoMapper<Turn,TurnDaoJson> turnDaoMapper)
            : base(resourceName: resourceName, turnDaoMapper: turnDaoMapper) { }

        public Operation<Turn> Insert(Turn turn)
        {
            Operation<List<Turn>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Turn>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperationResult.Result);
            _writeCache = readCache.ToList();
            TurnDaoJson turnDao = daoMapper.ToDAO(turn);
            _writeCache.Add(turnDao);
            TurnDaosCollection collection = new TurnDaosCollection(_writeCache.ToArray());
            string data = new TurnDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this.path, data);
            Operation<Turn> getOperation = Get(userId: turn.User.Id, roundId: turn.Round.Id);
            return getOperation.WasOk ? getOperation : Operation<Turn>.Failure(errorMessage: "failure to save Turn");
        }

        public Operation<Turn> Update(Turn turn)
        {
            Operation<List<Turn>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Turn>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperationResult.Result);
            _writeCache = readCache.ToList();
            TurnDaoJson turnDao = daoMapper.ToDAO(turn);

            int indexOfObjectToUpdate = _writeCache
                .FindIndex(dao => dao.UserId == turn.User.Id && dao.RoundId == turn.Round.Id);

            _writeCache.RemoveAt(indexOfObjectToUpdate);

            turnDao = new TurnDaoJson(
                userId: turn.User.Id,
                roundId: turn.Round.Id,
                points: turn.Points,
                startDateTime: turn.StartDateTime.ToString("s"), //ISO 8601
                endDateTime: turn.EndDateTime == null ? "" : ((DateTime)turn.EndDateTime).ToString("s")); //ISO 8601

            _writeCache.Add(turnDao);
            TurnDaosCollection collection = new TurnDaosCollection(_writeCache.ToArray());
            string data = new TurnDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(path, data);
            Operation<Turn> getOperation = Get(userId: turnDao.UserId, roundId: turnDao.RoundId);

            return getOperation.WasOk ? getOperation : Operation<Turn>
                .Failure(errorMessage: $"failure to update Turn of User with Id {turn.User.Id} " +
                $"and round with id {turn.Round.Id}");
        }
    }
}
