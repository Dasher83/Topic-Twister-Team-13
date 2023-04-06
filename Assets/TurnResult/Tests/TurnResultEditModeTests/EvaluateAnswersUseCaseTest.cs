using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.Shared.Structs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Structs;
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
        List<RoundAnswer> roundAnswers = new List<RoundAnswer>()
        {
            new RoundAnswer("1", "", 0),
            new RoundAnswer("2", "", 1),
            new RoundAnswer("3", "", 2),
            new RoundAnswer("4", "", 3),
            new RoundAnswer("5", "", 4)
        };
        AnswersToEvaluateStruct answersToEvaluateStruct = new AnswersToEvaluateStruct(initialLetter, roundAnswers);

        List<EvaluatedAnswerStruct> actualResult = useCase.EvaluateAnswers(answersToEvaluateStruct);

        List<EvaluatedAnswerStruct> expectedResult = new List<EvaluatedAnswerStruct>();
        foreach (RoundAnswer answer in roundAnswers)
        {
            expectedResult.Add(new EvaluatedAnswerStruct(answer.CategoryId, answer.UserInput, isCorrect: false, order: answer.Order));
        }

        Assert.AreEqual(expectedResult, actualResult);
    }
}
