using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class RoundDaosCollectionDeserializer : IDeserializer<RoundDaosCollection>
    {
        private RoundDaosCollection _serializedObject;

        public RoundDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<RoundDaosCollection>(data);
            return _serializedObject;
        }
    }
}
