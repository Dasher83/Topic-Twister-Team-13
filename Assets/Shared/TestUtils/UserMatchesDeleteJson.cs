using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.TestUtils
{
    public class UserMatchesDeleteJson
    {
        private string _path;

        public UserMatchesDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/UserMatches.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/UserMatches").text;
            List<UserMatchDaoJson> userMatches = JsonUtility.FromJson<UserMatchDaosCollection>(originalData).UserMatches;
            userMatches.Clear();
            UserMatchDaosCollection collection = new UserMatchDaosCollection(userMatches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}