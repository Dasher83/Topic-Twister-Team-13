namespace TopicTwister.Shared.Interfaces
{
    public interface ISerializer<T>
    {
        T Serialize(string data);
    }
}
