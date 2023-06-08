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
    public class UserRoundsReadOnlyRepository : IUserRoundsReadOnlyRepository
    {
        protected string path;
        protected List<UserRoundDaoJson> readCache;
        protected IdaoMapper<UserRound, UserRoundDaoJson> daoMapper;

        public UserRoundsReadOnlyRepository(
            string resourceName,
            IdaoMapper<UserRound, UserRoundDaoJson> userRoundDaoMapper)
        {
            path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            daoMapper = userRoundDaoMapper;
            readCache = userRoundDaoMapper.ToDAOs(GetAll().Result);
        }

        public Operation<UserRound> Get(int userId, int roundId)
        {
            Operation<List<UserRound>> GetAllOperation = GetAll();
            if (GetAllOperation.WasOk == false)
            {
                return Operation<UserRound>.Failure(errorMessage: GetAllOperation.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperation.Result);

            UserRoundDaoJson dao = readCache.SingleOrDefault(
                userRound => userRound.UserId == userId && userRound.RoundId == roundId);

            if (dao == null)
            {
                return Operation<UserRound>.Failure(errorMessage:
                    $"UserRound not found with userId: {userId} and roundId: {roundId}");
            }

            UserRound entity = daoMapper.FromDAO(dao);
            return Operation<UserRound>.Success(result: entity);
        }

        public Operation<List<UserRound>> GetAll()
        {
            string data = File.ReadAllText(path);
            readCache = new UserRoundDaosCollectionDeserializer().Deserialize(data).UserRounds;
            List<UserRound> entities = daoMapper.FromDAOs(readCache.ToList());
            return Operation<List<UserRound>>.Success(result: entities);
        }

        public Operation<List<UserRound>> GetMany(int userId, List<int> roundIds)
        {
            Operation<List<UserRound>> GetAllOperation = GetAll();

            if (GetAllOperation.WasOk == false)
            {
                return Operation<List<UserRound>>.Failure(errorMessage: GetAllOperation.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperation.Result);
            List<UserRoundDaoJson> daos = readCache
                .Where(dao => dao.UserId == userId && roundIds.Contains(dao.RoundId))
                .ToList();

            return Operation<List<UserRound>>.Success(result: daoMapper.FromDAOs(daos));
        }
    }
}
