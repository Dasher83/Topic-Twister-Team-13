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
        protected readonly IdaoMapper<Turn, TurnDaoJson> daoMapper;
        protected List<TurnDaoJson> readCache;

        public TurnsReadOnlyRepositoryJson(
            string resourceName,
            IdaoMapper<Turn, TurnDaoJson> turnDaoMapper)
        {
            path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            this.daoMapper = turnDaoMapper;
            readCache = this.daoMapper.ToDAOs(GetAll().Result);
        }

        public Operation<Turn> Get(int userId, int roundId)
        {
            Operation<List<Turn>> GetAllOperation = GetAll();
            if (GetAllOperation.WasOk == false)
            {
                return Operation<Turn>.Failure(errorMessage: GetAllOperation.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperation.Result);

            TurnDaoJson turnDao = readCache.SingleOrDefault(
                turn => turn.UserId == userId && turn.RoundId == roundId);

            if (turnDao == null)
            {
                return Operation<Turn>.Failure(errorMessage: $"Turn not found with userId: {userId} and roundId: {roundId}");
            }

            Turn turn = daoMapper.FromDAO(turnDao);

            return Operation<Turn>.Success(result: turn);
        }

        public Operation<List<Turn>> GetAll()
        {
            string data = File.ReadAllText(path);
            readCache = new TurnDaosCollectionDeserializer().Deserialize(data).Turns;
            List<Turn> turns = daoMapper.FromDAOs(readCache.ToList());
            return Operation<List<Turn>>.Success(result: turns);
        }

        public Operation<List<Turn>> GetMany(int userId, Match match)
        {
            Operation<List<Turn>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<List<Turn>>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperationResult.Result);

            List<Turn> turns = readCache
                .Where(dao => dao.UserId == userId && match.Rounds.Select(round => round.Id).Contains(dao.RoundId))
                .Distinct()
                .Select(daoMapper.FromDAO)
                .ToList();

            return Operation<List<Turn>>.Success(result: turns);
        }
    }
}
