using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.PlayTurn.Presenters
{
    public class EndTurnPresenter : IEndTurnPresenter
    {
        private IPlayTurnView _view;

        public EndTurnPresenter(IPlayTurnView view)
        {
            _view = view;
            _view.EndTurn += EndTurnEventHandler;
        }

        public void EndTurnEventHandler(int userId, int roundId, AnswerDto[] answerDtos)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateView(EndOfTurnDto endOfTurnDto)
        {
            _view.ReceiveUpdateFromEndTurn(endOfTurnDto);
        }
    }
}
