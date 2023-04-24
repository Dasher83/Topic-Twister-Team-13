using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class MatchDaosCollectionDeserializer : IDeserializer<MatchDaosCollection>
    {
        private MatchDaosCollection _serializedObject;

        public MatchDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<MatchDaosCollection>(data);
            return _serializedObject;
        }
    }
}
