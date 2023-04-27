using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.UseCases.Utils;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IEvaluateAnswersUseCase
    {
        UseCaseResult<List<EvaluatedAnswerDTO>> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
    }
}
