using System.Collections.Generic;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;


namespace TopicTwister.TurnResult.Services
{
    public class AnswersEvaluationService : IAnswersEvaluationService
    {
        private readonly IEvaluateAnswersUseCase _useCase;

        private AnswersEvaluationService() { }

        public AnswersEvaluationService(IEvaluateAnswersUseCase evaluateAnswersUseCase) 
        {
            _useCase = evaluateAnswersUseCase;
        }

        public List<EvaluatedAnswerDTO> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate)
        {
            return _useCase.EvaluateAnswers(answerToEvaluate);
        }
    }
}
