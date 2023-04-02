using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Structs;

public interface IAnswersEvaluationService
{
    public List<EvaluatedAnswerStruct> EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate);
}
