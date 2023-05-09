using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.TurnResult.Commands
{
    public class EvaluateAnswersCommand : ICommand<ITurnResultPresenter>
    {
        private ITurnResultPresenter _turnResultPresenter;
        private readonly IAnswersEvaluationService _answersEvaluationService;
        private List<AnswerDto> _turnAnswers;
        private char _initialLetter;

        public List<AnswerDto> TurnAnswers
        {
            set => _turnAnswers = value;
        }

        public char InitialLetter
        {
            set => _initialLetter = value;
        }

        public ITurnResultPresenter Presenter { set => _turnResultPresenter = value; }

        public EvaluateAnswersCommand(IAnswersEvaluationService answersEvaluationService)
        {
            _answersEvaluationService = answersEvaluationService;
        }
        
        public void Execute()
        {
            if (_turnAnswers == null || _turnAnswers.Count == 0) throw new ArgumentNullException();
            if (_turnResultPresenter == null) throw new ArgumentNullException();

            AnswersToEvaluateDTO answersEvaluationService = new AnswersToEvaluateDTO(
                _initialLetter, _turnAnswers);

            _turnResultPresenter.EvaluatedAnswers = _answersEvaluationService.EvaluateAnswers(
                answersEvaluationService);
        }
    }
}
