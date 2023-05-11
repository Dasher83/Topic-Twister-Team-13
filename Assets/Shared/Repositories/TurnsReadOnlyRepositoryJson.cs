using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Utils;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class TurnsReadOnlyRepositoryJson : ITurnsReadOnlyRepository
    {
        protected readonly string path;
        protected readonly IdaoMapper<Turn, TurnDaoJson> turnDaoMapper;
        protected List<TurnDaoJson> readCache;

        public TurnsReadOnlyRepositoryJson(
            string resourceName,
            IdaoMapper<Turn, TurnDaoJson> turnDaoMapper)
        {
            path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            this.turnDaoMapper = turnDaoMapper;
            readCache = this.turnDaoMapper.ToDAOs(GetAll().Result);
        }

        public Operation<Turn> Get(int userId, int roundId)
        {
            Operation<List<Turn>> GetAllOperation = GetAll();
            if (GetAllOperation.WasOk == false)
            {
                return Operation<Turn>.Failure(errorMessage: GetAllOperation.ErrorMessage);
            }

            readCache = turnDaoMapper.ToDAOs(GetAllOperation.Result);

            TurnDaoJson turnDao = readCache.SingleOrDefault(
                turn => turn.UserId == userId && turn.RoundId == roundId);

            if (turnDao == null)
            {
                return Operation<Turn>.Failure(errorMessage: $"Turn not found with userId: {userId} and roundId: {roundId}");
            }
            Turn turn = turnDaoMapper.FromDAO(turnDao);

            return Operation<Turn>.Success(result: turn);
        }

        public Operation<List<Turn>> GetAll()
        {
            string data = File.ReadAllText(path);
            readCache = new TurnDaosCollectionDeserializer().Deserialize(data).Turns;
            List<Turn> turns = turnDaoMapper.FromDAOs(readCache.ToList());
            return Operation<List<Turn>>.Success(result: turns);
        }

        public Operation<List<Turn>> GetMany(int userId, int matchId)
        {
            throw new NotImplementedException();
        }
    }
}
