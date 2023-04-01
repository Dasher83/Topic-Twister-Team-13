using System;
using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Structs;


namespace TopicTwister.ResultRound.Shared.Interfaces
{
    public interface IResultRoundView
    {
        void LoadCategoryResultList();
        event Action OnLoad;
        public void EvaluateAnswers(List<EvaluatedAnswerStruct> evaluatedAnswers);
    }
}
