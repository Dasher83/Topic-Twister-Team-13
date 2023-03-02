using TopicTwister.NewRound.Actions;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Providers;


namespace TopicTwister.NewRound.Presenters
{
    public class ShuffleLetterPresenter : IShuffleLetterPresenter
    {
        private readonly IShuffleLetterView _shuffleLetterView;
        private IAction _getRandomLetterAction;
        
        private ShuffleLetterPresenter(){}

        public ShuffleLetterPresenter(IShuffleLetterView shuffleLetterView)
        {
            _shuffleLetterView = shuffleLetterView;
            GetShuffledLetterAction action = new ActionProvider<GetShuffledLetterAction>().Provide();
            action.ShuffleLetterPresenter = this;
            this.Action = action;
        }

        public IAction Action { set { _getRandomLetterAction = value; } }

        public void GetRandomLetter()
        {
            _getRandomLetterAction.Execute();
        }

        public void ShowLetter(string letter)
        {
            _shuffleLetterView.ShowLetter(letter);
        }
    }
}
