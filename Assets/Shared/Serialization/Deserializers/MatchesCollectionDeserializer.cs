using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class MatchesCollectionDeserializer : IDeserializer<MatchesCollection>
    {
        private MatchesCollection _serializedObject;

        public MatchesCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<MatchesCollection>(data);
            return _serializedObject;
        }
    }
}
