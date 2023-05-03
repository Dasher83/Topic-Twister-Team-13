using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Commands
{
    public class ResumeMatchCommand : ICommand<IResumeMatchPresenter>
    {

        private IResumeMatchPresenter _presenter;
        private readonly IResumeMatchGatewayService _gatewayService;

        private MatchDto _matchDto;

        public IResumeMatchPresenter Presenter
        {
            private get => _presenter;
            set => _presenter = value;
        }

        public ResumeMatchCommand(IResumeMatchGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        public void Execute()
        {
            RoundWithCategoriesDto roundWithCategoriesDto = _gatewayService.Create(matchDto: _matchDto);
            _presenter.UpdateView(roundWithCategoriesDto);
        }
    }
}
