using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Commands
{
    public class StartTurnCommand : ICommand<IStartTurnPresenter>
    {
        public IStartTurnPresenter Presenter
        {
            set => throw new System.NotImplementedException();
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
