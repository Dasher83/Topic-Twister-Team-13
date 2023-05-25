using TopicTwister.PlayTurn.Commands;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.PlayTurn.Shared.Providers;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Presenters
{
    public class EndTurnPresenter : IEndTurnPresenter
    {
        private IPlayTurnView _view;
        private ICommand<IEndTurnPresenter> _endTurnCommand;

        public EndTurnPresenter(IPlayTurnView view)
        {
            _view = view;
            _view.EndTurn += EndTurnEventHandler;
            _endTurnCommand = new CommandProvider<EndTurnCommand>().Provide();
            _endTurnCommand.Presenter = this;
        }

        public void EndTurnEventHandler(int userId, int roundId, AnswerDto[] answerDtos)
        {
            ((EndTurnCommand)_endTurnCommand).Configure(userId, roundId, answerDtos);
            _endTurnCommand.Execute();
        }

        public void UpdateView(EndOfTurnDto endOfTurnDto)
        {
            _view.ReceiveUpdateFromEndTurn(endOfTurnDto);
        }
    }
}
