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
    public class AnswersReadOnlyRepositoryJson : IAnswersReadOnlyRepository
    {
        protected readonly string path;
        protected readonly IdaoMapper<Answer, AnswerDaoJson> daoMapper;
        protected List<AnswerDaoJson> readCache;

        public AnswersReadOnlyRepositoryJson(
            string resourceName,
            IdaoMapper<Answer, AnswerDaoJson> daoMapper)
        {
            path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            this.daoMapper = daoMapper;
            readCache = this.daoMapper.ToDAOs(GetAll().Result);
        }

        public Operation<List<Answer>> GetAll()
        {
            string data = File.ReadAllText(path);
            readCache = new AnswerDaosCollectionDeserializer().Deserialize(data).Answers;
            List<Answer> answers = daoMapper.FromDAOs(readCache.ToList());
            return Operation<List<Answer>>.Success(result: answers);
        }

        public Operation<Answer> Get(int userId, int roundId, int categoryId)
        {
            Operation<List<Answer>> getAllOperation = GetAll();

            if(getAllOperation.WasOk == false)
            {
                return Operation<Answer>.Failure(errorMessage: getAllOperation.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(getAllOperation.Result);

            AnswerDaoJson dao = readCache
                .SingleOrDefault(dao => dao.UserId == userId && dao.RoundId == roundId && dao.CategoryId == categoryId);

            if (dao == null)
            {
                string errorMessage = $"Answer not found for user with id {userId} " +
                    $"and round with id {roundId} and category with id {categoryId}";

                return Operation<Answer>.Failure(errorMessage: errorMessage);
            }

            Answer answer = daoMapper.FromDAO(dao);
            return Operation<Answer>.Success(result: answer);
        }

        public Operation<List<Answer>> GetMany(int userId, int roundId)
        {
            Operation<List<Answer>> getAllOperation = GetAll();

            if (getAllOperation.WasOk == false)
            {
                return Operation<List<Answer>>.Failure(errorMessage: getAllOperation.ErrorMessage);
            }

            readCache = daoMapper.ToDAOs(getAllOperation.Result);

            List<AnswerDaoJson> daos = readCache
                .Where(dao => dao.UserId == userId && dao.RoundId == roundId)
                .ToList();

            List<Answer> answers = daoMapper.FromDAOs(daos);
            return Operation<List<Answer>>.Success(result: answers);
        }
    }
}
