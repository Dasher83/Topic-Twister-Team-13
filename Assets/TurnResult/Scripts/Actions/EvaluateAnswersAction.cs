using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.DTOs;


namespace TopicTwister.TurnResult.Actions
{
    public class EvaluateAnswersAction : IAction
    {
        private IResultRoundPresenter _resultRoundPresenter;
        private readonly IAnswersEvaluationService _answersEvaluationService;
        private List<RoundAnswerDTO> _roundAnswers;
        private char _initialLetter;

        public List<RoundAnswerDTO> RoundAnswers
        {
            set => _roundAnswers = value;
        }

        public char InitialLetter
        {
            set => _initialLetter = value;
        }

        public IResultRoundPresenter ResultRoundPresenter
        {
            set => _resultRoundPresenter = value;
        }

        public EvaluateAnswersAction(IAnswersEvaluationService answersEvaluationService)
        {
            _answersEvaluationService = answersEvaluationService;
        }
        
        public void Execute()
        {
            if (_roundAnswers == null || _roundAnswers.Count == 0) throw new ArgumentNullException();
            if (_resultRoundPresenter == null) throw new ArgumentNullException();

            AnswersToEvaluateDTO answersEvaluationService = new AnswersToEvaluateDTO(_initialLetter, _roundAnswers);
            
            _resultRoundPresenter.EvaluatedAnswers = _answersEvaluationService.EvaluateAnswers(answersEvaluationService);
        }
    }
}
