using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.Shared.Utils;


namespace TopicTwister.Shared.Repositories
{
    public class UserRoundsRepository: UserRoundsReadOnlyRepository, IUserRoundsRepository
    {
        private List<UserRoundDaoJson> _writeCache;

        public UserRoundsRepository(
            string resourceName,
            IdaoMapper<UserRound, UserRoundDaoJson> userRoundDaoJsonMapper) : base(resourceName, userRoundDaoJsonMapper) {}

        public Operation<UserRound> Insert(UserRound userRound)
        {
            Operation<List<UserRound>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<UserRound>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperationResult.Result);
            _writeCache = readCache.ToList();
            UserRoundDaoJson dao = daoMapper.ToDAO(userRound);
            _writeCache.Add(dao);
            UserRoundDaosCollection collection = new UserRoundDaosCollection(_writeCache.ToArray());
            string data = new UserRoundDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this.path, data);
            Operation<UserRound> getOperation = Get(userId: dao.UserId, roundId: dao.RoundId);
            return getOperation.WasOk ? getOperation : Operation<UserRound>.Failure(errorMessage: "failure to save UserRound");
        }
    }
}
