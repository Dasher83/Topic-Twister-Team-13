using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class UserMatchesCollectionDeserializer : IDeserializer<UserMatchesCollection>
    {
        private UserMatchesCollection _serializedObject;

        public UserMatchesCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<UserMatchesCollection>(data);
            return _serializedObject;
        }
    }
}
