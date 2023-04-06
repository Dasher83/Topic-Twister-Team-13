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
        #region -- Arrange --
        IEvaluateAnswersUseCase useCase = new EvaluateAnswersUseCase();
        char initialLetter = 'A';
        List<TurnAnswerDTO> roundAnswers = new List<TurnAnswerDTO>()
        {
            new TurnAnswerDTO(categoryId: "1", userInput: "", order: 0),
            new TurnAnswerDTO(categoryId: "2", userInput: "", order: 1),
            new TurnAnswerDTO(categoryId: "3", userInput: "", order: 2),
            new TurnAnswerDTO(categoryId: "4", userInput: "", order: 3),
            new TurnAnswerDTO(categoryId: "5", userInput: "", order: 4),
        };
        AnswersToEvaluateDTO answersToEvaluateStruct = new AnswersToEvaluateDTO(initialLetter, roundAnswers);
        #endregion

        #region -- Act --
        List<EvaluatedAnswerDTO> actualResult = useCase.EvaluateAnswers(answersToEvaluateStruct);
        #endregion

        #region -- Assert --
        List<EvaluatedAnswerDTO> expectedResult = new List<EvaluatedAnswerDTO>();
        foreach (TurnAnswerDTO answer in roundAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerDTO(
                answer.CategoryId,
                answer.UserInput,
                isCorrect: false,
                order: answer.Order));
        }

        Assert.AreEqual(expectedResult, actualResult);
        #endregion
    }
}
