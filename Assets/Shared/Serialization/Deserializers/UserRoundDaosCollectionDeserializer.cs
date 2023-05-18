using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class UserRoundDaosCollectionDeserializer : IDeserializer<UserRoundDaosCollection>
    {
        private UserRoundDaosCollection _serializedObject;

        public UserRoundDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<UserRoundDaosCollection>(data);
            return _serializedObject;
        }
    }
}
