using TopicTwister.PlayTurn.Shared.Interfaces;
using TopicTwister.PlayTurn.Views;


namespace TopicTwister.PlayTurn.Presenters
{
    public class StartTurnPresenter: IStartTurnPresenter
    {
        private IPlayTurnView _view;

        public StartTurnPresenter(IPlayTurnView view)
        {
            _view = view;
        }
    }
}