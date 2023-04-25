using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Commands
{
    public class CreateRoundCommand : ICommand<ICreateRoundPresenter>
    {

        private ICreateRoundPresenter _presenter;

        public ICreateRoundPresenter Presenter
        {
            private get => _presenter;
            set => _presenter = value;
        }

        public void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
