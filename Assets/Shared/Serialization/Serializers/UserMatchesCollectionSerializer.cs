using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class UserMatchesCollectionSerializer : ISerializer<UserMatchesCollection>
    {
        private string _data;

        public string Serialize(UserMatchesCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}