using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface ITurnResultPresenter
    {
        void EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
        List<EvaluatedAnswerDTO> EvaluatedAnswers { set; }
    }
}
