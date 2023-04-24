namespace TopicTwister.Shared.Interfaces
{
    public interface ISerializer<T>
    {
        string Serialize(T objectToSerialize);
    }
}