using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Structs;


namespace TopicTwister.ResultRound.Shared.Interfaces
{
    public interface IResultRoundPresenter
    {
        public List<EvaluatedAnswerStruct> EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate);
    }
}
