using TopicTwister.Home.Scripts.Commands;
using TopicTwister.Home.Scripts.Shared.Interfaces;
using TopicTwister.Home.Scripts.Shared.Providers;
using TopicTwister.Shared.Interfaces;

namespace TopicTwister.Home.Presenters
{
    public class NewBotMatchPresenter : INewBotMatchPresenter
    {
        private INewBotMatchView _view;
        private ICommand _createMatchCommand;

        public NewBotMatchPresenter(INewBotMatchView view)
        {
            _view = view;
            _view.StartMatchVersusBot += CreateMatchVersusBot;
            CreateNewBotMatchCommand command = new CommandProvider<CreateNewBotMatchCommand>().Provide();
            command.Presenter = this;
            _createMatchCommand = command;
        }

        public void CreateMatchVersusBot()
        {
            _createMatchCommand.Execute();
        }
    }
}
