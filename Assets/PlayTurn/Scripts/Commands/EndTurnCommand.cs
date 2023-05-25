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
        private int? _userId;
        private int? _roundId;
        private AnswerDto[] _answerDtos;
    
        public IEndTurnPresenter Presenter
        {
            set => _presenter = value;
        }

        public void Configure(int userId, int roundId, AnswerDto[] answerDtos)
        {
            _userId = userId;
            _roundId = roundId;
            _answerDtos = answerDtos;
        }

        public void Execute()
        {
            CheckConfiguration();

            //TODO: Implement gateway
            throw new NotImplementedException();
        }

        private void CheckConfiguration()
        {
            if (_presenter == null) 
                throw new ArgumentException();
            if (_userId == null || _roundId == null || _answerDtos == null) 
                throw new ArgumentNullException();
            if (_answerDtos.Length != Configuration.CategoriesPerRound || _answerDtos.Any(answer => answer == null))
                throw new ArgumentNullException();
        }
    }
}
