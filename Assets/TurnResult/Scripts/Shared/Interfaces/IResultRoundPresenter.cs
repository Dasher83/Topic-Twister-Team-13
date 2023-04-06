using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.Structs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IResultRoundPresenter
    {
        void EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate);
        List<EvaluatedAnswerStruct> EvaluatedAnswers { set; }
    }
}
