using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Home.Tests.Utils
{
    public class UserMatchesDeleteJson
    {
        private string _path;

        public UserMatchesDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/UserMatchesTest.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/UserMatchesTest").text;
            List<UserMatchDTO> userMatches = JsonUtility.FromJson<UserMatchesCollection>(originalData).UserMatches;
            userMatches.Clear();
            UserMatchesCollection collection = new UserMatchesCollection(userMatches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }

        public void Delete(int userId, int matchId)
        {string originalData = Resources.Load<TextAsset>($"JSON/TestData/UserMatchesTest").text;
            List<UserMatchDTO> userMatches = JsonUtility.FromJson<UserMatchesCollection>(originalData).UserMatches;
            UserMatchDTO userMatchToDelete = userMatches.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            userMatches.Remove(userMatchToDelete);
            UserMatchesCollection collection = new UserMatchesCollection(userMatches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}