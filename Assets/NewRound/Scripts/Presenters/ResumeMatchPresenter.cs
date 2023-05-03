using System;
using TopicTwister.NewRound.Commands;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Providers;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Presenters
{
    public class ResumeMatchPresenter : IResumeMatchPresenter
    {
        private ICreateRoundView _view;
        private ICommand<IResumeMatchPresenter> _createRoundCommand;

        public ResumeMatchPresenter(ICreateRoundView view)
        {
            _view = view;
            _createRoundCommand = new CommandProvider<ResumeMatchCommand, IResumeMatchPresenter>().Provide();
            _createRoundCommand.Presenter = this;
            _view.OnLoad += ResumeMatch;
        }

        public void ResumeMatch(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateView(RoundWithCategoriesDto roundWithCategoriesDto)
        {
            _view.UpdateNewRoundData(roundWithCategoriesDto: roundWithCategoriesDto);
        }
    }
}
