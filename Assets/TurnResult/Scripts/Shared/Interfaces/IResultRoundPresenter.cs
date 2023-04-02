using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Structs;


namespace TopicTwister.ResultRound.Shared.Interfaces
{
    public interface IResultRoundPresenter
    {
        void EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate);
        List<EvaluatedAnswerStruct> EvaluatedAnswers { set; }
    }
}
