using TopicTwister.Home.Scripts.Shared.Interfaces;


namespace TopicTwister.Home.Presenters
{
    public class NewBotMatchPresenter : INewBotMatchPresenter
    {
        private INewBotMatchView _view;

        public NewBotMatchPresenter(INewBotMatchView view)
        {
            _view = view;
            _view.StartMatchVersusBot += CreateMatchVersusBot;
        }

        public void CreateMatchVersusBot()
        {
            throw new System.NotImplementedException();
        }
    }
}
