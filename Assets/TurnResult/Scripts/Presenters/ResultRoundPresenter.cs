using System.Collections.Generic;
using TopicTwister.TurnResult.Actions;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Providers;
using TopicTwister.TurnResult.Shared.Structs;


namespace TopicTwister.TurnResult.Presenters
{
    public class ResultRoundPresenter : IResultRoundPresenter
    {
        private readonly IResultRoundView _resultRoundView;
        private EvaluateAnswersAction _evaluateAnswerAction;
        private List<EvaluatedAnswerStruct> _evaluatedAnswers;

        public List<EvaluatedAnswerStruct> EvaluatedAnswers {
            set
            {
                _evaluatedAnswers = value;
                _resultRoundView.EvaluateAnswers(_evaluatedAnswers);
            }
        }

        public ResultRoundPresenter(IResultRoundView resultRoundView)
        {
            _resultRoundView = resultRoundView;
            _resultRoundView.OnLoad += OnLoadHandler;
            _evaluateAnswerAction = new ActionProvider<EvaluateAnswersAction>().Provide();
            _evaluateAnswerAction.ResultRoundPresenter = this;
        }

        ~ResultRoundPresenter()
        {
            this._resultRoundView.OnLoad -= OnLoadHandler;
        }

        private void OnLoadHandler()
        {
            EvaluateAnswers(_resultRoundView.GetAnswersToEvaluate());
        }
        
        public void EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate)
        {
            _evaluateAnswerAction.RoundAnswers = answerToEvaluate.roundAnswers;
            _evaluateAnswerAction.InitialLetter = answerToEvaluate.initialLetter;
            _evaluateAnswerAction.Execute();
        }
    }
}
