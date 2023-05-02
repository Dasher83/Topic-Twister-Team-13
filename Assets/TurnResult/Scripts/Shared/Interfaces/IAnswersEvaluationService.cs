using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IAnswersEvaluationService
    {
        public List<EvaluatedAnswerDto> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
    }
}
