using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Deserializers;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class UserMatchesRepositoryJson : IUserMatchesRepository
    {
        private string _path;
        private List<UserMatchDTO> _userMatches;
        List<UserMatchDTO> matchesToWriteCache; //TODO : cambiar nombre

        public UserMatchesRepositoryJson(string matchesResourceName)
        {
            _path = $"{Application.dataPath}/Resources/JSON/{matchesResourceName}.json";
            _userMatches = GetAll();
        }

        public UserMatchDTO Create(int userId, int matchId)
        {
            _userMatches = GetAll();
            UserMatchDTO userMatch = new UserMatchDTO(
                score: 0, isWinner: false, hasInitiative: true,
                userId: userId, matchId: matchId);
            //TODO: Continuar aca mirando como referencia el repositorio de matches 
            return userMatch; //Esto es de borrador, no deberia devolverse esta variable, deberia devolverse una nueva variable traida de la bbdd
        }

        public UserMatchDTO Get(int userId, int matchId)
        {
            throw new System.NotImplementedException();
        }

        public List<UserMatchDTO> GetAll()
        {
            string data = File.ReadAllText(_path);
            _userMatches = new UserMatchesCollectionDeserializer().Deserialize(data).UserMatches;
            return _userMatches.ToList();
        }
    }
}
