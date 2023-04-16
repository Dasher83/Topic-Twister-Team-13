using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Serialization;
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
            List<MatchDTO> matches = JsonUtility.FromJson<MatchesCollection>(originalData).Matches;
            matches.Clear();
            MatchesCollection collection = new MatchesCollection(matches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }

        public void Delete(int id)
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/MatchesTest").text;
            List<MatchDTO> matches = JsonUtility.FromJson<MatchesCollection>(originalData).Matches;
            MatchDTO matchToDelete = matches.SingleOrDefault(match => match.Id == id);
            matches.Remove(matchToDelete);
            MatchesCollection collection = new MatchesCollection(matches.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
