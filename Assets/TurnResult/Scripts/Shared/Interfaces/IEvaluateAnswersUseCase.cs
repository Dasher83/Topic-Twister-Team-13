using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IEvaluateAnswersUseCase
    {
        Result<List<EvaluatedAnswerDTO>> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
    }
}
