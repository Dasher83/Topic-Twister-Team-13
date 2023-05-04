using System.IO;
using System.Linq;
using System.Collections.Generic;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Utils;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Deserializers;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesReadOnlyRepositoryJson : IMatchesReadOnlyRepository
    {
        protected string _path;
        protected List<MatchDaoJson> _readCache;
        protected IdaoMapper<Match, MatchDaoJson> _matchDaoMapper;

        public MatchesReadOnlyRepositoryJson(
            string resourceName,
            IdaoMapper<Match, MatchDaoJson> matchDaoMapper)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{resourceName}.json";
            _matchDaoMapper = matchDaoMapper;
            _readCache = _matchDaoMapper.ToDAOs(GetAll().Outcome);
        }

        public Operation<List<Match>> GetAll()
        {
            string data = File.ReadAllText(_path);
            _readCache = new MatchDaosCollectionDeserializer().Deserialize(data).Matches;
            List<Match> matches = _matchDaoMapper.FromDAOs(_readCache.ToList());
            return Operation<List<Match>>.Success(outcome: matches);
        }

        public Operation<Match> Get(int id)
        {
            Operation<List<Match>> GetAllOperationResult = GetAll();
            if(GetAllOperationResult.WasOk == false)
            {
                return Operation<Match>.Failure(errorMessage: GetAllOperationResult.ErrorMessage);
            }

            _readCache = _matchDaoMapper.ToDAOs(GetAllOperationResult.Outcome);
            MatchDaoJson matchDAO = _readCache.SingleOrDefault(match => match.Id == id && match.Id >= 0);
            if(matchDAO == null)
            {
                return Operation<Match>.Failure(errorMessage: $"Match not found with id: {id}");

            }
            Match match = _matchDaoMapper.FromDAO(matchDAO);

            return Operation<Match>.Success(outcome: match);
        }
    }
}
