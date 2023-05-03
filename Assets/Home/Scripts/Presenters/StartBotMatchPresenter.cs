using TopicTwister.Home.Commands;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Home.Shared.Providers;
using TopicTwister.Shared.Interfaces;

namespace TopicTwister.Home.Presenters
{
    public class StartBotMatchPresenter : IStartBotMatchPresenter
    {
        private IStartBotMatchView _view;
        private ICommand<IStartBotMatchPresenter> _createMatchCommand;

        public StartBotMatchPresenter(IStartBotMatchView view)
        {
            _view = view;
            _view.StartMatchVersusBot += StartMatchVersusBot;
            _createMatchCommand = new CommandProvider<StartNewBotMatchCommand, IStartBotMatchPresenter>().Provide();
            _createMatchCommand.Presenter = this;
        }

        public void StartMatchVersusBot()
        {
            _createMatchCommand.Execute();
        }
    }
}
