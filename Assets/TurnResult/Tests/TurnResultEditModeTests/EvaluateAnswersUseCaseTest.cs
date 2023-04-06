using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Structs;
using TopicTwister.TurnResult.UseCases;
using UnityEngine;
using UnityEngine.TestTools;


public class EvaluateAnswersUseCaseTest
{
    [Test]
    public void EvaluateAnswersUseCaseTestSimplePasses()
    {
        IEvaluateAnswersUseCase useCase = new EvaluateAnswersUseCase();
        useCase.EvaluateAnswers(new AnswersToEvaluateStruct());
    }
}
