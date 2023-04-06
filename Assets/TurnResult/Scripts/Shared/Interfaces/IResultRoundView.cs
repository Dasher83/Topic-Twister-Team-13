using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IResultRoundView
    {
        event Action OnLoad;
        public void EvaluateAnswers(List<EvaluatedAnswerDTO> evaluatedAnswers);
        public AnswersToEvaluateDTO GetAnswersToEvaluate();
    }
}
