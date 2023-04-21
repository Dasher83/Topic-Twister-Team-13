using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class MatchesDaoCollectionSerializer : ISerializer<MatchesDaoCollection>
    {
        private string _data;

        public string Serialize(MatchesDaoCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}
