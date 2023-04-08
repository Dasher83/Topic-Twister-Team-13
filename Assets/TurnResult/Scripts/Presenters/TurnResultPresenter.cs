using System.Collections.Generic;
using TopicTwister.TurnResult.Actions;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Providers;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Presenters
{
    public class TurnResultPresenter : ITurnResultPresenter
    {
        private readonly ITurnResultView _turnResultView;
        private EvaluateAnswersAction _evaluateAnswerAction;
        private List<EvaluatedAnswerDTO> _evaluatedAnswers;

        public List<EvaluatedAnswerDTO> EvaluatedAnswers {
            set
            {
                _evaluatedAnswers = value;
                _turnResultView.EvaluateAnswers(_evaluatedAnswers);
            }
        }

        public TurnResultPresenter(ITurnResultView turnResultView)
        {
            _turnResultView = turnResultView;
            _turnResultView.OnLoad += OnLoadHandler;
            _evaluateAnswerAction = new ActionProvider<EvaluateAnswersAction>().Provide();
            _evaluateAnswerAction.TurnResultPresenter = this;
        }

        ~TurnResultPresenter()
        {
            this._turnResultView.OnLoad -= OnLoadHandler;
        }

        private void OnLoadHandler()
        {
            EvaluateAnswers(_turnResultView.GetAnswersToEvaluate());
        }
        
        public void EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate)
        {
            _evaluateAnswerAction.TurnAnswers = answerToEvaluate.turnAnswers;
            _evaluateAnswerAction.InitialLetter = answerToEvaluate.initialLetter;
            _evaluateAnswerAction.Execute();
        }
    }
}
