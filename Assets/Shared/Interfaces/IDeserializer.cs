namespace TopicTwister.Shared.Interfaces
{
    public interface IDeserializer<T>
    {
        T Deserialize(string data);
    }
}
