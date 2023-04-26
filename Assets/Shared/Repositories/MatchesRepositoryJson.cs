using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;
using TopicTwister.Shared.DAOs;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesRepositoryJson : MatchesReadOnlyRepositoryJson, IMatchesRepository
    {
        private string _path;
        private List<MatchDaoJson> _matchesReadCache;
        private List<MatchDaoJson> _matchesWriteCache;
        private IUniqueIdGenerator _idGenerator;
        private IdaoMapper<Match, MatchDaoJson> _mapper;

        public MatchesRepositoryJson(string matchesResourceName, IUniqueIdGenerator idGenerator): base(matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _mapper = new MatchDaoJsonMapper();
            _matchesReadCache = _mapper.ToDAOs(GetAll());
            _idGenerator = idGenerator;
        }

        public Match Save(Match match)
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
                throw new MatchNotSavedByRepositoryException();
            }
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
