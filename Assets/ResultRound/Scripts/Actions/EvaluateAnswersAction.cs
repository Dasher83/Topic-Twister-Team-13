using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Structs;


namespace TopicTwister.ResultRound.Actions
{
    public class EvaluateAnswersAction : IAction
    {
        private readonly IResultRoundPresenter _resultRoundPresenter;
        private List<RoundAnswer> _roundAnswers;

        public List<RoundAnswer> RoundAnswers
        {
            set => _roundAnswers = value;
        }

        public EvaluateAnswersAction(IResultRoundPresenter resultRoundPresenter)
        {
            _resultRoundPresenter = resultRoundPresenter;
        }
        
        //Recibir struct del SO con los datos
        public void Execute()
        {
            if (_roundAnswers == null || _roundAnswers.Count == 0) throw new ArgumentNullException();
            throw new System.NotImplementedException();
        }
    }
}
