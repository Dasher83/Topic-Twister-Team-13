namespace TopicTwister.Shared.Interfaces
{
    public interface ICommand<P>
    {
        void Execute();

        P Presenter { set; }
    }
}
