using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization;
using UnityEngine;


namespace TopicTwister.Shared.Repositories
{
    public class MatchesRepositoryJson : IMatchesRepository
    {
        private const string MatchesResourceName = "Matches";

        private readonly List<MatchDTO> _matches;

        public MatchesRepositoryJson()
        {
            string data = Resources.Load<TextAsset>(MatchesResourceName).text;

            _matches = JsonUtility.FromJson<MatchesCollection>(data).Matches;
        }

        public MatchDTO Create(int userOneId, int userTwoId)
        {
            // TO DO
            throw new System.NotImplementedException();
        }
    }
}
