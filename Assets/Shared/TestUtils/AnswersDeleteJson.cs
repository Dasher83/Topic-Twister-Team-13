using System.Collections.Generic;
using System.IO;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.TestUtils
{
    public class AnswersDeleteJson
    {
        private string _path;

        public AnswersDeleteJson()
        {
            _path = $"{Application.dataPath}/Resources/JSON/TestData/Answers.json";
        }

        public void Delete()
        {
            string originalData = Resources.Load<TextAsset>($"JSON/TestData/Answers").text;
            List<AnswerDaoJson> answerDaos = JsonUtility.FromJson<AnswerDaosCollection>(originalData).Answers;
            answerDaos.Clear();
            AnswerDaosCollection collection = new AnswerDaosCollection(answerDaos.ToArray());
            string newData = JsonUtility.ToJson(collection);
            File.WriteAllText(this._path, newData);
        }
    }
}
