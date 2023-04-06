using System;
using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.Structs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IResultRoundView
    {
        event Action OnLoad;
        public void EvaluateAnswers(List<EvaluatedAnswerStruct> evaluatedAnswers);
        public AnswersToEvaluateStruct GetAnswersToEvaluate();
    }
}
