using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class UserRoundDaosCollectionSerializer : ISerializer<UserRoundDaosCollection>
    {
        private string _data;

        public string Serialize(UserRoundDaosCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}
