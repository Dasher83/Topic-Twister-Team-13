using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Interfaces;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Structs;


namespace TopicTwister.ResultRound.Actions
{
    public class EvaluateAnswersAction : IAction
    {
        private IResultRoundPresenter _resultRoundPresenter;
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

        public EvaluateAnswersAction()
        {
            // Todo: inject service
        }
        
        //Recibir struct del SO con los datos
        public void Execute()
        {
            if (_roundAnswers == null || _roundAnswers.Count == 0) throw new ArgumentNullException();
            if (_resultRoundPresenter == null) throw new ArgumentNullException();

            // Todo: call gateway service 

            // Todo: pass a value here from the gateway service returns
            _resultRoundPresenter.EvaluatedAnswers = null;
        }
    }
}
