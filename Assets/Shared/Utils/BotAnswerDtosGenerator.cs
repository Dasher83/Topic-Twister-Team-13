using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Shared.Utils
{
    public class BotAnswerDtosGenerator : IBotAnswerDtosGenerator
    {

        public List<AnswerDto> GenerateAnswers(List<AnswerDto> userAnswerDtos, char initialLetter)
        {
            List<AnswerDto> generatedAnswers = new List<AnswerDto>();
            bool willAnswerCorrectly;
            AnswerDto botAnswer;


            foreach (AnswerDto answerDto in userAnswerDtos)
            {
                willAnswerCorrectly = UnityEngine.Random.value > 0.5f;
                if (willAnswerCorrectly)
                {
                    botAnswer = new AnswerDto(
                        categoryDto: answerDto.CategoryDto,
                        userInput: $"{initialLetter} test".ToUpper(),
                        order: answerDto.Order,
                        isCorrect: true);
                }
                else
                {
                    botAnswer = new AnswerDto(
                        categoryDto: answerDto.CategoryDto,
                        userInput: $"No se".ToUpper(),
                        order: answerDto.Order,
                        isCorrect: false);
                }
                generatedAnswers.Add(botAnswer);
            }

            return generatedAnswers;
        }
    }
}
