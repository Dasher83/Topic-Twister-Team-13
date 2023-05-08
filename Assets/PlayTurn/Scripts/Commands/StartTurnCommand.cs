using System;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Commands
{
    public class StartTurnCommand : ICommand<IStartTurnPresenter>
    {
        private IStartTurnPresenter _presenter;
        private IStartTurnGatewayService _gatewayService;
        private int? _userId;
        private int? _matchId;

        public StartTurnCommand(IStartTurnGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        public int UserId
        {
            set => _userId = value;
        }

        public int MatchId
        {
            set => _matchId = value;
        }

        public IStartTurnPresenter Presenter
        {
            set => _presenter = value;
        }

        public void Execute()
        {
            if (_userId == null || _matchId == null) throw new ArgumentNullException();

            TurnDto turnDto = _gatewayService.StartTurn(
                userId: (int)_userId, matchId: (int)_matchId);

            _presenter.UpdateView(turnDto);
        }
    }
}
