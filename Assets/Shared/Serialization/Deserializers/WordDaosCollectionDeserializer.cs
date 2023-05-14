using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Shared;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Deserializers
{
    public class WordDaosCollectionDeserializer : IDeserializer<WordDaosCollection>
    {
        private WordDaosCollection _serializedObject;

        public WordDaosCollection Deserialize(string data)
        {
            _serializedObject = JsonUtility.FromJson<WordDaosCollection>(data);
            return _serializedObject;
        }
    }
}
