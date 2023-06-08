using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class AnswerDaosCollectionDeserializer : IDeserializer<AnswerDaosCollection>
    {
        private AnswerDaosCollection _serializedObject;

        public AnswerDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<AnswerDaosCollection>(data);
            return _serializedObject;
        }
    }
}
