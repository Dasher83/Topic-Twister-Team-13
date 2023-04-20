using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Mappers;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;
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
        private List<MatchDTO> _matchesReadCache;
        private List<MatchDTO> matchesWriteCache;
        private IUniqueIdGenerator _idGenerator;
        private MatchMapper _mapper;

        public MatchesRepositoryJson(string matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _mapper = new MatchMapper();
            _matchesReadCache = _mapper.ToDTOs(GetAll());
            _idGenerator = new MatchesIdGenerator(matchesRepository: this);
        }

        public Match Persist(Match match)
        {
            _matchesReadCache = _mapper.ToDTOs(GetAll());
            matchesWriteCache = _matchesReadCache.ToList();
            MatchDTO matchDto = _mapper.ToDTO(match);

            matchDto = new MatchDTO(id: _idGenerator.GetNextId(),
                startDateTime: matchDto.StartDateTime,
                endDateTime: matchDto.EndDateTime);

            matchesWriteCache.Add(matchDto);
            MatchesCollection collection = new MatchesCollection(matchesWriteCache.ToArray());
            string data = new MatchesCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            return Get(matchDto.Id); // TODO atrapar excepciones de negocio personalizadas y relanzar desconocidas
        }

        public List<Match> GetAll()
        {
            string data = File.ReadAllText(_path);
            _matchesReadCache = new MatchesCollectionDeserializer().Deserialize(data).Matches;
            List<Match> matches = _mapper.FromDTOs(_matchesReadCache.ToList());
            return matches;
        }

        public Match Get(int id)
        {
            _matchesReadCache = _mapper.ToDTOs(GetAll());
            MatchDTO matchDTO = _matchesReadCache.SingleOrDefault(match => match.Id == id);
            Match match = _mapper.FromDTO(matchDTO);
            return match;
        }
    }
}
