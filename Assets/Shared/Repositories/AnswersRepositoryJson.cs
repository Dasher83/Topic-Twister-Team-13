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
    public class AnswersRepositoryJson : AnswersReadOnlyRepositoryJson, IAnswersRepository
    {
        private List<AnswerDaoJson> _writeCache;

        public AnswersRepositoryJson(string resourceName, IdaoMapper<Answer,AnswerDaoJson> daoMapper)
            : base(resourceName: resourceName, daoMapper: daoMapper) { }

        public Operation<Answer> Insert(Answer answer)
        {
            Operation<List<Answer>> GetAllOperationResult = GetAll();
            if (GetAllOperationResult.WasOk == false)
            {
                return Operation<Answer>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(GetAllOperationResult.Result);
            _writeCache = readCache.ToList();
            AnswerDaoJson dao = daoMapper.ToDAO(answer);
            _writeCache.Add(dao);
            AnswerDaosCollection collection = new AnswerDaosCollection(_writeCache.ToArray());
            string data = new AnswerDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this.path, data);

            Operation<Answer> getOperation = Get(
                userId: answer.Turn.User.Id,
                roundId: answer.Turn.Round.Id,
                categoryId: answer.Category.Id);

            return getOperation.WasOk ? getOperation : Operation<Answer>.Failure(errorMessage: "failure to insert Answer");
        }
    }
}
