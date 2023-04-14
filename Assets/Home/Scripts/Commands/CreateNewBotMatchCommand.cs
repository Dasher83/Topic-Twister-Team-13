using System;
using TopicTwister.Home.Scripts.Shared.Interfaces;
using TopicTwister.Home.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.Scripts.Commands
{
    public class CreateNewBotMatchCommand : ICommand
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
            MatchDTO match = _service.Create();
        }
    }
}
