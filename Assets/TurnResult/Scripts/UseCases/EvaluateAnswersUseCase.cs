using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.UseCases
{
    public class EvaluateAnswersUseCase : IEvaluateAnswersUseCase
    {
        public List<EvaluatedAnswerDTO> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate)
        {
            List<EvaluatedAnswerDTO> result = new List<EvaluatedAnswerDTO>();
            EvaluatedAnswerDTO evaluatedAnswer;
            bool isCorrect;

            foreach(RoundAnswerDTO roundAnswer in answerToEvaluate.roundAnswers)
            {
                isCorrect = !string.IsNullOrEmpty(roundAnswer.UserInput);

                evaluatedAnswer = new EvaluatedAnswerDTO(
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
