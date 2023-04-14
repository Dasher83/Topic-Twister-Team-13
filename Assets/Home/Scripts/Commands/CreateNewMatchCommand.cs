using System;
using TopicTwister.Home.Scripts.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Home.Scripts.Commands
{
    public class CreateNewMatchCommand : ICommand
    {
        private INewBotMatchPresenter _presenter;

        public INewBotMatchPresenter Presenter { set => _presenter = value; }

        public CreateNewMatchCommand()
        {
            //
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
