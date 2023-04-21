using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class UserMatchDaosCollectionSerializer : ISerializer<UserMatchDaosCollection>
    {
        private string _data;

        public string Serialize(UserMatchDaosCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}