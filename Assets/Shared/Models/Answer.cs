using TopicTwister.Shared.Constants;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Shared.Models
{
    public class Answer
    {
        private string _userInput;
        private int _order;
        private Category _category;
        private Turn _turn;

        public string UserInput => _userInput;
        public int Order => _order;
        public Category Category => _category;
        public Turn Turn => _turn;

        private Answer() { }

        public Answer(string userInput, int order, Category category, Turn turn)
        {
            _order = order;
            _category = category;
            _turn = turn;

            if(_turn.DurationInSeconds > Configuration.TurnDurationInSecondsPlusTolerance)
            {
                _userInput = "";
            }
            else
            {
                _userInput = userInput;
            }
        }

        public bool IsCorrect(IWordsRepository wordsRepository)
        {
            return wordsRepository.Exists(
                text: _userInput,
                categoryId: _category.Id,
                initialLetter: _turn.Round.InitialLetter).Result;
        }
    }
}
