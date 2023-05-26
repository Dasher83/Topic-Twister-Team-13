using System;
using System.Linq;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Commands
{
    public class EndTurnCommand : ICommand<IEndTurnPresenter>
    {
        private IEndTurnPresenter _presenter;
        private IEndTurnGatewayService _gatewayService;
        private int? _userId;
        private int? _matchId;
        private AnswerDto[] _answerDtos;

        private EndTurnCommand() { }

        public EndTurnCommand(IEndTurnGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        public IEndTurnPresenter Presenter
        {
            set => _presenter = value;
        }

        public void Configure(int userId, int matchId, AnswerDto[] answerDtos)
        {
            _userId = userId;
            _matchId = matchId;
            _answerDtos = answerDtos;
        }

        public void Execute()
        {
            CheckConfiguration();

            EndOfTurnDto endOfTurnDto = _gatewayService
                .EndTurn(userId: (int)_userId, matchId: (int)_matchId, answerDtos: _answerDtos);

            _presenter.UpdateView(endOfTurnDto: endOfTurnDto);
        }

        private void CheckConfiguration()
        {
            if (_presenter == null) 
                throw new ArgumentException();
            if (_userId == null || _matchId == null || _answerDtos == null) 
                throw new ArgumentNullException();
            if (_answerDtos.Length != Configuration.CategoriesPerRound || _answerDtos.Any(answer => answer == null))
                throw new ArgumentNullException();
        }
    }
}
