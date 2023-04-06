using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface IResultRoundPresenter
    {
        void EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
        List<EvaluatedAnswerDTO> EvaluatedAnswers { set; }
    }
}
