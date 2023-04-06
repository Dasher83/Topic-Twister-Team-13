using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.TurnResult.UseCases;
using UnityEngine;
using UnityEngine.TestTools;


public class EvaluateAnswersUseCaseTest
{
    [Test]
    public void Test_evaluate_all_empty_answers()
    {
        IEvaluateAnswersUseCase useCase = new EvaluateAnswersUseCase();
        char initialLetter = 'A';
        List<RoundAnswerDTO> roundAnswers = new List<RoundAnswerDTO>()
        {
            new RoundAnswerDTO("1", "", 0),
            new RoundAnswerDTO("2", "", 1),
            new RoundAnswerDTO("3", "", 2),
            new RoundAnswerDTO("4", "", 3),
            new RoundAnswerDTO("5", "", 4)
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, roundAnswers);

        List<EvaluatedAnswerDTO> actualResult = useCase.EvaluateAnswers(answersToEvaluateStruct);

        List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
        foreach (RoundAnswerDTO answer in roundAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerDTO(answer.CategoryId, answer.UserInput, isCorrect: false, order: answer.Order));
        }

        Assert.AreEqual(expectedResult, actualResult);
    }
}
