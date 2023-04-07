using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;


namespace TopicTwister.TurnResult.UseCases
{
    public class EvaluateAnswersUseCase : IEvaluateAnswersUseCase
    {
        private IWordsRepository _wordRepository;

        private EvaluateAnswersUseCase() { }

        public EvaluateAnswersUseCase(IWordsRepository wordsRepository)
        {
            _wordRepository = wordsRepository;
        }

        public List<EvaluatedAnswerDTO> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate)
        {
            List<EvaluatedAnswerDTO> result = new List<EvaluatedAnswerDTO>();
            EvaluatedAnswerDTO evaluatedAnswer;
            bool isCorrect;

            foreach(TurnAnswerDTO turnAnswer in answerToEvaluate.turnAnswers)
            {
                if (string.IsNullOrEmpty(turnAnswer.UserInput))
                {
                    isCorrect = false;
                }
                else
                {
                    isCorrect = _wordRepository.Exists(
                        text: turnAnswer.UserInput,
                        categoryId: turnAnswer.CategoryId,
                        initialLetter: answerToEvaluate.initialLetter);
                }

                evaluatedAnswer = new EvaluatedAnswerDTO(
                    category: turnAnswer.CategoryId,
                    answer: turnAnswer.UserInput,
                    isCorrect: isCorrect,
                    order: turnAnswer.Order);

                result.Add(evaluatedAnswer);
            }

            return result;
        }
    }
}
