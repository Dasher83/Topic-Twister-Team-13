using System.Collections.Generic;
using System.IO;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.TestUtils
{
    public class RoundsDeleteJson
    {
        private string _path;

        public RoundsDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/Rounds.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/Rounds").text;
            List<RoundDaoJson> roundDaos = JsonUtility.FromJson<RoundDaosCollection>(originalData).RoundDaos;
            roundDaos.Clear();
            RoundDaosCollection collection = new RoundDaosCollection(roundDaos.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
