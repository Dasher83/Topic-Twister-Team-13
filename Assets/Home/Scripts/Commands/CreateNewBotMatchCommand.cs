using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.Commands
{
    public class CreateNewBotMatchCommand : ICommand<INewBotMatchPresenter>
    {
        private INewBotMatchPresenter _presenter;

        private ICreateBotMatchService _service;

        public INewBotMatchPresenter Presenter { set => _presenter = value; }

        public CreateNewBotMatchCommand(ICreateBotMatchService service)
        {
            _service = service;
        }

        public void Execute()
        {
            MatchDto match = _service.Create();
        }
    }
}
