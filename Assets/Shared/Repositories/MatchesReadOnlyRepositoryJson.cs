using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Deserializers;
using UnityEngine;
using TopicTwister.Shared.DAOs;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesReadOnlyRepositoryJson : IMatchesReadOnlyRepository
    {
        private string _path;
        private List<MatchDaoJson> _matchesReadCache;
        private IdaoMapper<Match, MatchDaoJson> _mapper;

        public MatchesReadOnlyRepositoryJson(string matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _mapper = new MatchDaoJsonMapper();
            _matchesReadCache = _mapper.ToDAOs(GetAll());
        }

        public List<Match> GetAll()
        {
            string data = File.ReadAllText(_path);
            _matchesReadCache = new MatchDaosCollectionDeserializer().Deserialize(data).Matches;
            List<Match> matches = _mapper.FromDAOs(_matchesReadCache.ToList());
            return matches;
        }

        public Match Get(int id)
        {
            _matchesReadCache = _mapper.ToDAOs(GetAll());
            MatchDaoJson matchDAO = _matchesReadCache.SingleOrDefault(match => match.Id == id);
            if(matchDAO == null)
            {
                throw new MatchNotFoundByRespositoryException();
            }
            Match match = _mapper.FromDAO(matchDAO);
            return match;
        }
    }
}
