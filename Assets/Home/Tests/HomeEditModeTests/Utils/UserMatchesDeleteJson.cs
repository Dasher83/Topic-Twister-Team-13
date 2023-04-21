using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
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
            List<UserMatchDaoJson> userMatches = JsonUtility.FromJson<UserMatchDaosCollection>(originalData).UserMatches;
            userMatches.Clear();
            UserMatchDaosCollection collection = new UserMatchDaosCollection(userMatches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }

        public void Delete(int userId, int matchId)
        {string originalData = Resources.Load<TextAsset>($"JSON/TestData/UserMatchesTest").text;
            List<UserMatchDaoJson> userMatches = JsonUtility.FromJson<UserMatchDaosCollection>(originalData).UserMatches;
            UserMatchDaoJson userMatchToDelete = userMatches.SingleOrDefault(
                userMatch => userMatch.UserId == userId && userMatch.MatchId == matchId);
            userMatches.Remove(userMatchToDelete);
            UserMatchDaosCollection collection = new UserMatchDaosCollection(userMatches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}