using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IEvaluateAnswersUseCase
    {
        List<EvaluatedAnswerDTO> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
    }
}
