using System.Collections.Generic;
using System.IO;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.TestUtils
{
    public class UserRoundsDeleteJson
    {
        private string _path;

        public UserRoundsDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/UserRounds.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/UserRounds").text;
            List<UserRoundDaoJson> daos = JsonUtility.FromJson<UserRoundDaosCollection>(originalData).UserRounds;
            daos.Clear();
            UserRoundDaosCollection collection = new UserRoundDaosCollection(daos.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
