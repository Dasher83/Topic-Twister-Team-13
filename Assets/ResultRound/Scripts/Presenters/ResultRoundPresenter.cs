using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Actions;
using TopicTwister.ResultRound.Shared.Interfaces;
using TopicTwister.ResultRound.Shared.Providers;
using TopicTwister.ResultRound.Shared.Structs;


namespace TopicTwister.ResultRound.Presenters
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
            // Todo: connect it to the proper method please, or call it inside this one for readability
            Console.WriteLine("OnLoad event received.");
        }
        
        public void EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate)
        {
            _evaluateAnswerAction.RoundAnswers = answerToEvaluate.roundAnswers;
            _evaluateAnswerAction.InitialLetter = answerToEvaluate.initialLetter;
            _evaluateAnswerAction.Execute();
        }
    }
}
