using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Commands
{
    public class CreateRoundCommand : ICommand<ICreateRoundPresenter>
    {

        private ICreateRoundPresenter _presenter;
        private readonly ICreateRoundGatewayService _gatewayService;

        private MatchDto _matchDto;

        public ICreateRoundPresenter Presenter
        {
            private get => _presenter;
            set => _presenter = value;
        }

        public CreateRoundCommand(ICreateRoundGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        public void Execute()
        {
            RoundWithCategoriesDto roundWithCategoriesDto = _gatewayService.Create(matchDto: _matchDto);
            _presenter.UpdateRound(roundWithCategoriesDto);
        }
    }
}
