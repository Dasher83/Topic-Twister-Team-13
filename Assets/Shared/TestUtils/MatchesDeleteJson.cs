using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.TestUtils
{
    public class MatchesDeleteJson
    {
        private string _path;

        public MatchesDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/Matches.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/Matches").text;
            List<MatchDaoJson> matches = JsonUtility.FromJson<MatchDaosCollection>(originalData).Matches;
            matches.Clear();
            MatchDaosCollection collection = new MatchDaosCollection(matches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
