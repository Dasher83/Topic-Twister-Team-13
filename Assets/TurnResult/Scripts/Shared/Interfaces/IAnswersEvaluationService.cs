using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IAnswersEvaluationService
    {
        public List<EvaluatedAnswerDTO> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
    }
}
