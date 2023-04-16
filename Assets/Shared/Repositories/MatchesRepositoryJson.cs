using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesRepositoryJson : IMatchesRepository
    {
        private string _path;
        private List<MatchDTO> _matches;
        List<MatchDTO> matchesToWriteCache;
        private IUniqueIdGenerator _idGenerator;

        public MatchesRepositoryJson(string matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _matches = GetAll();
            _idGenerator = new MatchesIdGenerator(matchesRepository: this);
        }

        public MatchDTO Create(int userOneId, int userTwoId)
        {
            _matches = GetAll();
            MatchDTO match = new MatchDTO(
                id: _idGenerator.GetNextId(),
                startDateTime: DateTime.UtcNow);
            matchesToWriteCache = _matches.ToList();
            matchesToWriteCache.Add(match);
            MatchesCollection collection = new MatchesCollection(matchesToWriteCache.ToArray());
            string data = new MatchesCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            MatchDTO newMatch = Get(match.Id);
            return newMatch;
        }

        public List<MatchDTO> GetAll()
        {
            string data = File.ReadAllText(_path);
            _matches = new MatchesCollectionDeserializer().Deserialize(data).Matches;
            return _matches.ToList();
        }

        public MatchDTO Get(int id)
        {
            _matches = GetAll();
            MatchDTO matchObtained = _matches.SingleOrDefault(match => match.Id == id);
            return matchObtained;
        }
    }
}
