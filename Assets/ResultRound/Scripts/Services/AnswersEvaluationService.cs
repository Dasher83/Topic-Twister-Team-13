using System.Collections.Generic;
using TopicTwister.ResultRound.Shared.Structs;
using TopicTwister.Shared.Structs;
using UnityEngine;


public class AnswersEvaluationService : IAnswersEvaluationService
{
    private List<EvaluatedAnswerStruct> evaluatedAnswerStructs;
    private bool isCorrect;
    
    public List<EvaluatedAnswerStruct> EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate)
    {
        evaluatedAnswerStructs = new List<EvaluatedAnswerStruct>();

        foreach (RoundAnswer roundAnswer in answerToEvaluate.roundAnswers)
        {
            isCorrect = Random.Range(0f, 1f) > 0.5f;

            evaluatedAnswerStructs.Add(new EvaluatedAnswerStruct(roundAnswer.CategoryId, roundAnswer.UserInput, isCorrect));
        }
        
        return evaluatedAnswerStructs;
    }
}
