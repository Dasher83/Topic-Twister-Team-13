using TopicTwister.NewRound.Commands;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Providers;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Presenters
{
    public class ResumeMatchPresenter : IResumeMatchPresenter
    {
        private IResumeMatchView _view;
        private ICommand<IResumeMatchPresenter> _createRoundCommand;

        public ResumeMatchPresenter(IResumeMatchView view)
        {
            _view = view;
            _createRoundCommand = new CommandProvider<ResumeMatchCommand>().Provide();
            _createRoundCommand.Presenter = this;
            _view.Load += ResumeMatch;
        }

        public void ResumeMatch(MatchDto matchDto)
        {
            ((ResumeMatchCommand)_createRoundCommand).MatchDto = matchDto;
            _createRoundCommand.Execute();
        }

        public void UpdateView(RoundWithCategoriesDto roundWithCategoriesDto)
        {
            _view.UpdateMatchData(roundWithCategoriesDto: roundWithCategoriesDto);
        }
    }
}
