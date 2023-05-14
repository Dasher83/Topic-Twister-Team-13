using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.TurnResult.Shared.Interfaces;
using TopicTwister.TurnResult.Shared.DTOs;
using TopicTwister.Shared.Utils;
using TopicTwister.Shared.Interfaces;


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

        public Operation<List<EvaluatedAnswerDto>> EvaluateAnswers(AnswersToEvaluateDTO answerToEvaluate)
        {
            List<EvaluatedAnswerDto> result = new List<EvaluatedAnswerDto>();
            EvaluatedAnswerDto evaluatedAnswer;
            bool isCorrect;

            foreach(AnswerDto turnAnswer in answerToEvaluate.turnAnswers)
            {
                if (string.IsNullOrEmpty(turnAnswer.UserInput))
                {
                    isCorrect = false;
                }
                else
                {
                    isCorrect = _wordRepository.Exists(
                        text: turnAnswer.UserInput,
                        categoryId: turnAnswer.CategoryDto.Id,
                        initialLetter: answerToEvaluate.initialLetter).Result;
                }

                evaluatedAnswer = new EvaluatedAnswerDto(
                    category: turnAnswer.CategoryDto,
                    answer: turnAnswer.UserInput,
                    isCorrect: isCorrect,
                    order: turnAnswer.Order);

                result.Add(evaluatedAnswer);
            }

            Operation<List<EvaluatedAnswerDto>> useCaseResult = Operation<List<EvaluatedAnswerDto>>.Success(result: result);
            return useCaseResult;
        }
    }
}
