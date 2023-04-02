using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Interfaces;
using TopicTwister.ResultRound.Shared.Structs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Structs;


namespace TopicTwister.ResultRound.Actions
{
    public class EvaluateAnswersAction : IAction
    {
        private IResultRoundPresenter _resultRoundPresenter;
        private readonly IAnswersEvaluationService _answersEvaluationService;
        private List<RoundAnswer> _roundAnswers;
        private char _initialLetter;

        public List<RoundAnswer> RoundAnswers
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

            AnswersToEvaluateStruct answersEvaluationService = new AnswersToEvaluateStruct(_initialLetter, _roundAnswers);
            
            _resultRoundPresenter.EvaluatedAnswers = _answersEvaluationService.EvaluateAnswers(answersEvaluationService);
        }
    }
}
