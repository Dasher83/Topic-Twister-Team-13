using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.NewRound.Actions
{
    public class GetShuffledLetterAction : IAction
    {
        private IShuffleLetterPresenter _shuffleLetterPresenter;
        private readonly ILetterService _letterService;

        private GetShuffledLetterAction(){}
        
        public GetShuffledLetterAction(ILetterService letterService)
        {
            _letterService = letterService;
        }
        
        public IShuffleLetterPresenter ShuffleLetterPresenter { set { _shuffleLetterPresenter = value; } }
        
        public void Execute()
        {
            string letter = _letterService.GetRandomLetter();
            _shuffleLetterPresenter.ShowLetter(letter);
        }
    }
}
