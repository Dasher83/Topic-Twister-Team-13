using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Repositories.IdGenerators;
using TopicTwister.Shared.Serialization;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesRepositoryJson : IMatchesRepository
    {
        private string _matchesResourceName;
        private string _path;

        private List<MatchDTO> _matches;
        List<MatchDTO> matchesToWriteCache;
        private IUniqueIdGenerator _idGenerator;

        public MatchesRepositoryJson(string matchesResourceName)
        {
            _matchesResourceName = matchesResourceName;
            _path = $"{Application.dataPath}/Resources/JSON/{_matchesResourceName}.json";
            _matches = GetAll();
            _idGenerator = new MatchesIdGenerator(matchesRepository: this);
        }

        public MatchDTO Create(int userOneId, int userTwoId)
        {
            MatchDTO match = new MatchDTO(
                id: _idGenerator.GetNextId(),
                startDateTime: DateTime.UtcNow);
            _matches = GetAll();
            matchesToWriteCache = _matches.ToList();
            matchesToWriteCache.Add(match);
            MatchesCollection collection = new MatchesCollection(matchesToWriteCache.ToArray());
            string data = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, data);
            MatchDTO newMatch = Get(match.Id);
            return newMatch;
        }

        public List<MatchDTO> GetAll()
        {
            string resourceName = $"JSON/{_matchesResourceName}";
            string data = Resources.Load<TextAsset>(resourceName).text;
            _matches = JsonUtility.FromJson<MatchesCollection>(data).Matches;
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
