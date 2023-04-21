using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;
using TopicTwister.Shared.DAOs;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesRepositoryJson : IMatchesRepository
    {
        private string _path;
        private List<MatchDaoJson> _matchesReadCache;
        private List<MatchDaoJson> _matchesWriteCache;
        private IUniqueIdGenerator _idGenerator;
        private IdaoMapper<Match, MatchDaoJson> _mapper;

        public MatchesRepositoryJson(string matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _mapper = new MatchDaoJsonMapper();
            _matchesReadCache = _mapper.ToDAOs(GetAll());
            _idGenerator = new MatchesIdGenerator(matchesRepository: this);
        }

        public Match Persist(Match match)
        {
            _matchesReadCache = _mapper.ToDAOs(GetAll());
            _matchesWriteCache = _matchesReadCache.ToList();
            MatchDaoJson matchDAO = _mapper.ToDAO(match);

            matchDAO = new MatchDaoJson(id: _idGenerator.GetNextId(),
                startDateTime: matchDAO.StartDateTime,
                endDateTime: matchDAO.EndDateTime);

            _matchesWriteCache.Add(matchDAO);
            MatchDaosCollection collection = new MatchDaosCollection(_matchesWriteCache.ToArray());
            string data = new MatchDaosCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            try
            {
                return Get(matchDAO.Id);
            }
            catch (MatchNotFoundByRespositoryException)
            {
                throw new MatchNotPersistedByRepositoryException();
            }
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

        public void Delete(int id)
        {
            MatchDaoJson matchToDelete = _mapper.ToDAO(Get(id));
            _matchesWriteCache = _matchesReadCache.ToList();
            _matchesWriteCache.Remove(matchToDelete);
            MatchDaosCollection collection = new MatchDaosCollection(_matchesWriteCache.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
