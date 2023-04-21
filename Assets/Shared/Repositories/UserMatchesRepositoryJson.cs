﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.Repositories.Exceptions;
using TopicTwister.Shared.Serialization.Deserializers;
using TopicTwister.Shared.Serialization.Serializers;
using TopicTwister.Shared.Serialization.Shared;
using TopicTwister.Shared.Mappers;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class UserMatchesRepositoryJson : IUserMatchesRepository
    {
        private string _path;
        private List<UserMatchDTO> _userMatchesWriteCache;
        private List<UserMatchDTO> _userMatchesReadCache;
        private UserMatchDtoMapper _mapper;
        private IMatchesRepository _matchRepository;

        public UserMatchesRepositoryJson(string userMatchesResourceName, IMatchesRepository matchesRepository)
        {
            _matchRepository = matchesRepository;
            _mapper = new UserMatchDtoMapper(matchesRepository: _matchRepository);
            _path = $"{Application.dataPath}/Resources/JSON/{userMatchesResourceName}.json";
            _userMatchesReadCache = _mapper.ToDTOs(GetAll());
        }

        public UserMatch Persist(UserMatch userMatch)
        {
            _userMatchesReadCache = _mapper.ToDTOs(GetAll());
            _userMatchesWriteCache = _userMatchesReadCache.ToList();
            UserMatchDTO userMatchDTO = _mapper.ToDTO(userMatch);
            _userMatchesWriteCache.Add(userMatchDTO);
            UserMatchesCollection collection = new UserMatchesCollection(_userMatchesWriteCache.ToArray());
            string data = new UserMatchesCollectionSerializer().Serialize(collection);
            File.WriteAllText(this._path, data);
            UserMatch newUserMatch;
            try
            {
                newUserMatch = Get(userId: userMatch.UserId, matchId: userMatch.Match.Id);
            }
            catch (UserMatchNotFoundByRepositoryException)
            {
                throw new UserMatchNotPersistedByRepositoryException();
            }
            return newUserMatch;
        }

        public UserMatch Get(int userId, int matchId)
        {
            _userMatchesReadCache = _mapper.ToDTOs(GetAll());
            UserMatchDTO userMatchObtained = _userMatchesReadCache.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            if (userMatchObtained == null)
            {
                throw new UserMatchNotFoundByRepositoryException(message: $"userId '{userId}' matchId '{matchId}'");
            }
            UserMatch userMatch = _mapper.FromDTO(userMatchObtained);
            return userMatch;
        }

        public List<UserMatch> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatchesReadCache = new UserMatchesCollectionDeserializer().Deserialize(data).UserMatches;
            List<UserMatch> userMatches = _mapper.FromDTOs(_userMatchesReadCache.ToList());
            return userMatches;
        }
    }
}
