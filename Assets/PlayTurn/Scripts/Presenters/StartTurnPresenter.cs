using TopicTwister.PlayTurn.Commands;
using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.PlayTurn.Shared.Providers;
using TopicTwister.PlayTurn.Views;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.PlayTurn.Presenters
{
    public class StartTurnPresenter: IStartTurnPresenter
    {
        private IPlayTurnView _view;
        private ICommand<IStartTurnPresenter> _startTurnCommand;

        public StartTurnPresenter(IPlayTurnView view)
        {
            _view = view;
            _view.Load += LoadEventHandler;
            _startTurnCommand = new CommandProvider<StartTurnCommand>().Provide();
            _startTurnCommand.Presenter = this;
        }

        public void LoadEventHandler(int userId, int matchId)
        {
            ((StartTurnCommand)_startTurnCommand).UserId = userId;
            ((StartTurnCommand)_startTurnCommand).MatchId = matchId;
            _startTurnCommand.Execute();
        }

        public void UpdateView(TurnDto turnDto)
        {
            _view.ReceiveUpdate(turnDto);
        }
    }
}
