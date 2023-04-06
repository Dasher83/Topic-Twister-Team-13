using System.Collections.Generic;
using TopicTwister.Shared.Structs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.Structs;


namespace TopicTwister.TurnResult.UseCases
{
    public class EvaluateAnswersUseCase : IEvaluateAnswersUseCase
    {
        public List<EvaluatedAnswerStruct> EvaluateAnswers(AnswersToEvaluateStruct answerToEvaluate)
        {
            List<EvaluatedAnswerStruct> result = new List<EvaluatedAnswerStruct>();
            EvaluatedAnswerStruct evaluatedAnswer;
            bool isCorrect;

            foreach(RoundAnswer roundAnswer in answerToEvaluate.roundAnswers)
            {
                isCorrect = !string.IsNullOrEmpty(roundAnswer.UserInput);

                evaluatedAnswer = new EvaluatedAnswerStruct(
                    category: roundAnswer.CategoryId,
                    answer: roundAnswer.UserInput,
                    isCorrect: isCorrect,
                    order: roundAnswer.Order);

                result.Add(evaluatedAnswer);
            }

            return result;
        }
    }
}
