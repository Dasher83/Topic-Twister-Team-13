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
        private IUniqueIdGenerator _idGenerator;

        public MatchesRepositoryJson(string matchesResourceName)
        {
            _matchesResourceName = matchesResourceName;
            _path = $"{Application.dataPath}/Resources/{_matchesResourceName}.json";
            _matches = GetAll();
            _idGenerator = new MatchesIdGenerator(matchesRepository: this);
        }

        public MatchDTO Create(int userOneId, int userTwoId)
        {
            MatchDTO match = new MatchDTO(
                id: _idGenerator.GetNextId(),
                startDateTime: DateTime.UtcNow);
            _matches = GetAll();
            _matches.Add(match);
            MatchesCollection collection = new MatchesCollection(_matches.ToArray());
            string data = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, data);
            return match;
        }

        public List<MatchDTO> GetAll()
        {
            string data = Resources.Load<TextAsset>(_matchesResourceName).text;
            _matches = JsonUtility.FromJson<MatchesCollection>(data).Matches;
            return _matches.ToList();
        }
    }
}
