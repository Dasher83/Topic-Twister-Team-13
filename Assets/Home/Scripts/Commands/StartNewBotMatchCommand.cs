using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.Commands
{
    public class StartNewBotMatchCommand : ICommand<IStartBotMatchPresenter>
    {
        private IStartBotMatchPresenter _presenter;

        private IStartBotMatchService _service;

        public IStartBotMatchPresenter Presenter { set => _presenter = value; }

        public StartNewBotMatchCommand(IStartBotMatchService service)
        {
            _service = service;
        }

        public void Execute()
        {
            MatchDto matchDto = _service.StartMatch();
            _presenter.UpdateView(matchDto);
        }
    }
}
