﻿using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Utils;
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
            Result<List<EvaluatedAnswerDTO>> useCaseResult = _useCase.EvaluateAnswers(answerToEvaluate);
            return useCaseResult.WasOk ? useCaseResult.Outcome : null;
        }
    }
}
