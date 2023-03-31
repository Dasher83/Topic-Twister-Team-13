using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Interfaces;
using TopicTwister.ResultRound.Shared.Structs;


namespace TopicTwister.ResultRound.Presenters
{
    public class ResultRoundPresenter : IResultRoundPresenter
    {
        private readonly IResultRoundView _resultRoundView;

        public ResultRoundPresenter(IResultRoundView resultRoundView)
        {
            _resultRoundView = resultRoundView;
            _resultRoundView.OnLoad += OnLoadHandler;
        }

        ~ResultRoundPresenter()
        {
            this._resultRoundView.OnLoad -= OnLoadHandler;
        }

        private void OnLoadHandler()
        {
            Console.WriteLine("OnLoad event received.");
        }
        
        public List<EvaluatedAnswerStruct> EvaluateAnswers(AnswerToEvaluateStruct answerToEvaluate)
        {
            List<EvaluatedAnswerStruct> evaluatedAnswers;
            
             //TODO return evaluatedAnswers;
             
            throw new NotImplementedException();
        }
    }
}
