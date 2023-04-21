using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Serializers
{
    public class MatchDaosCollectionSerializer : ISerializer<MatchDaosCollection>
    {
        private string _data;

        public string Serialize(MatchDaosCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}
