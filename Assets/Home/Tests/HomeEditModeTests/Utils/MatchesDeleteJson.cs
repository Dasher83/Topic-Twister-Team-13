using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Home.Tests.Utils
{
    public class MatchesDeleteJson
    {
        private string _path;

        public MatchesDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/MatchesTest.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/MatchesTest").text;
            List<MatchDaoJson> matches = JsonUtility.FromJson<MatchesDaoCollection>(originalData).Matches;
            matches.Clear();
            MatchesDaoCollection collection = new MatchesDaoCollection(matches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }

        public void Delete(int id)
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/MatchesTest").text;
            List<MatchDaoJson> matches = JsonUtility.FromJson<MatchesDaoCollection>(originalData).Matches;
            MatchDaoJson matchToDelete = matches.SingleOrDefault(match => match.Id == id);
            matches.Remove(matchToDelete);
            MatchesDaoCollection collection = new MatchesDaoCollection(matches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
