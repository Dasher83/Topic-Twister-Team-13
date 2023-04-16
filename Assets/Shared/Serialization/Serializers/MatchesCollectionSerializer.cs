using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class MatchesCollectionSerializer : ISerializer<MatchesCollection>
    {
        private MatchesCollection _serializedObject;

        public MatchesCollection Serialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<MatchesCollection>(data);
            return _serializedObject;
        }
    }
}
