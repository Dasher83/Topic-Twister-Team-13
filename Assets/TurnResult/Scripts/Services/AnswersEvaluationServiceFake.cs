using System.Collections.Generic;
using System.Linq;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Structs;
using UnityEngine;


namespace TopicTwister.TurnResult.Services
{
    public class AnswersEvaluationServiceFake : IAnswersEvaluationService
    {
        private List<EvaluatedAnswerStruct> evaluatedAnswerStructs;
        private bool isCorrect;

        public List<EvaluatedAnswerStruct> EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate)
        {
            evaluatedAnswerStructs = new List<EvaluatedAnswerStruct>();

            for (int i = 0; i < answerToEvaluate.roundAnswers.Count; i++)
            {
                isCorrect = Random.Range(0f, 1f) > 0.5f;

                evaluatedAnswerStructs.Add(
                    new EvaluatedAnswerStruct(
                        answerToEvaluate.roundAnswers[i].CategoryId,
                        answerToEvaluate.roundAnswers[i].UserInput,
                        isCorrect,
                        order: answerToEvaluate.roundAnswers[i].Order));
            }

            return evaluatedAnswerStructs.OrderBy(item => item.order).ToList();
        }
    }
}
