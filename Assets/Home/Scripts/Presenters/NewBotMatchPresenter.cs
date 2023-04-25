using TopicTwister.Home.Commands;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.Shared.Providers;
using TopicTwister.Shared.Interfaces;

namespace TopicTwister.Home.Presenters
{
    public class NewBotMatchPresenter : INewBotMatchPresenter
    {
        private INewBotMatchView _view;
        private ICommand<INewBotMatchPresenter> _createMatchCommand;

        public NewBotMatchPresenter(INewBotMatchView view)
        {
            _view = view;
            _view.StartMatchVersusBot += CreateMatchVersusBot;
            _createMatchCommand = new CommandProvider<CreateNewBotMatchCommand, INewBotMatchPresenter>().Provide();
            _createMatchCommand.Presenter = this;
        }

        public void CreateMatchVersusBot()
        {
            _createMatchCommand.Execute();
        }
    }
}
