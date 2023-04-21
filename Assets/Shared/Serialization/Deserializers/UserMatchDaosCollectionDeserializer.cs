using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class UserMatchDaosCollectionDeserializer : IDeserializer<UserMatchDaosCollection>
    {
        private UserMatchDaosCollection _serializedObject;

        public UserMatchDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<UserMatchDaosCollection>(data);
            return _serializedObject;
        }
    }
}
