using System;
using System.Collections.Generic;
using TopicTwister.Shared.Constants;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.Shared.Interfaces
{
    public interface ITurnResultView
    {
        event EventDelegates.ITurnResultView.LoadEventHandler Load;
        public void EvaluateAnswers(List<EvaluatedAnswerDto> evaluatedAnswers);
        public AnswersToEvaluateDTO GetAnswersToEvaluate();
    }
}
