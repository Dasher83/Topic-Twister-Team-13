using System.Collections.Generic;
using System.IO;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.TestUtils
{
    public class TurnsDeleteJson
    {
        private string _path;

        public TurnsDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/Turns.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/Turns").text;
            List<TurnDaoJson> turnDaos = JsonUtility.FromJson<TurnDaosCollection>(originalData).Turns;
            turnDaos.Clear();
            TurnDaosCollection collection = new TurnDaosCollection(turnDaos.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
