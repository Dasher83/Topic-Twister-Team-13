using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class MatchesDaoCollectionDeserializer : IDeserializer<MatchesDaoCollection>
    {
        private MatchesDaoCollection _serializedObject;

        public MatchesDaoCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<MatchesDaoCollection>(data);
            return _serializedObject;
        }
    }
}
