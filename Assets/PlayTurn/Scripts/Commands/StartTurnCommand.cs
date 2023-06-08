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

        public void Configure(int userId, int matchId)
        {
            _userId = userId;
            _matchId = matchId;
        }

        public IStartTurnPresenter Presenter
        {
            set => _presenter = value;
        }

        public void Execute()
        {
            CheckConfiguration();

            TurnDto turnDto = _gatewayService.StartTurn(
                userId: (int)_userId, matchId: (int)_matchId);

            _presenter.UpdateView(turnDto);
        }
        
        private void CheckConfiguration()
        {
            if (_presenter == null) 
                throw new ArgumentException();
            if (_userId == null || _matchId == null) 
                throw new ArgumentNullException();
        }
    }
}
