using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class AnswerDaosCollectionSerializer : ISerializer<AnswerDaosCollection>
    {
        private string _data;

        public string Serialize(AnswerDaosCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}
