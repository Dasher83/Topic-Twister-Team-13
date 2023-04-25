using System;
using TopicTwister.NewRound.Commands;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Providers;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Presenters
{
    public class CreateRoundPresenter : ICreateRoundPresenter
    {
        private ICreateRoundView _view;
        private ICommand<ICreateRoundPresenter> _createRoundCommand;

        public CreateRoundPresenter(ICreateRoundView view)
        {
            _view = view;
            _createRoundCommand = new CommandProvider<CreateRoundCommand, ICreateRoundPresenter>().Provide();
            _createRoundCommand.Presenter = this;
            _view.OnLoad += CreateRound;
        }

        public void CreateRound(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateRound(RoundWithCategoriesDto roundWithCategoriesDto)
        {
            _view.UpdateNewRoundData(roundWithCategoriesDto: roundWithCategoriesDto);
        }
    }
}
