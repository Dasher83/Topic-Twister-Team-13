using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IEvaluateAnswersUseCase
    {
        Operation<List<EvaluatedAnswerDto>> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
    }
}
