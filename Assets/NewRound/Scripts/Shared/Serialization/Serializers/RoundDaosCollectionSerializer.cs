using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.NewRound.Shared.Serialization.Serializers
{
    public class RoundDaosCollectionSerializer : ISerializer<RoundDaosCollection>
    {
        private string _data;

        public string Serialize(RoundDaosCollection objectToSerialize)
        {
            _data = JsonUtility.ToJson(objectToSerialize);
            return _data;
        }
    }
}
