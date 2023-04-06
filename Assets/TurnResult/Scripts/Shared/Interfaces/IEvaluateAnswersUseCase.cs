using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.Structs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IEvaluateAnswersUseCase
    {
        List<EvaluatedAnswerStruct> EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate);
    }
}
