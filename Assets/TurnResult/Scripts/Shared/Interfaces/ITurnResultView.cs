using System;
using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface ITurnResultView
    {
        event Action OnLoad;
        public void EvaluateAnswers(List<EvaluatedAnswerDto> evaluatedAnswers);
        public AnswersToEvaluateDTO GetAnswersToEvaluate();
    }
}
