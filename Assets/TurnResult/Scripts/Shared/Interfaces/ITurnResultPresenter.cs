using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface ITurnResultPresenter
    {
        void EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate);
        List<EvaluatedAnswerDto> EvaluatedAnswers { set; }
    }
}
