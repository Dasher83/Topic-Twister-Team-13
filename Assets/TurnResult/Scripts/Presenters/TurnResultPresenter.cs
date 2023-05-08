using System.Collections.Generic;
using TopicTwister.TurnResult.Actions;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Providers;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.TurnResult.Presenters
{
    public class TurnResultPresenter : ITurnResultPresenter
    {
        private readonly ITurnResultView _turnResultView;
        private ICommand<ITurnResultPresenter> _evaluateAnswerCommand;
        private List<EvaluatedAnswerDto> _evaluatedAnswers;

        public List<EvaluatedAnswerDto> EvaluatedAnswers {
            set
            {
                _evaluatedAnswers = value;
                _turnResultView.EvaluateAnswers(_evaluatedAnswers);
            }
        }

        public TurnResultPresenter(ITurnResultView turnResultView)
        {
            _turnResultView = turnResultView;
            _turnResultView.Load += LoadEventHandler;
            _evaluateAnswerCommand = new CommandProvider<EvaluateAnswersAction>().Provide();
            _evaluateAnswerCommand.Presenter = this;
        }

        ~TurnResultPresenter()
        {
            this._turnResultView.Load -= LoadEventHandler;
        }

        private void LoadEventHandler()
        {
            EvaluateAnswers(_turnResultView.GetAnswersToEvaluate());
        }
        
        public void EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate)
        {
            ((EvaluateAnswersAction)_evaluateAnswerCommand).TurnAnswers = answerToEvaluate.turnAnswers;
            ((EvaluateAnswersAction)_evaluateAnswerCommand).InitialLetter = answerToEvaluate.initialLetter;
            _evaluateAnswerCommand.Execute();
        }
    }
}
