using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class TurnDaosCollectionDeserializer : IDeserializer<TurnDaosCollection>
    {
        private TurnDaosCollection _serializedObject;

        public TurnDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<TurnDaosCollection>(data);
            return _serializedObject;
        }
    }
}
