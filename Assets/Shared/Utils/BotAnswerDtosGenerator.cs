using System.Collections.Generic;
using System.IO;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Serialization.Deserializers;
using UnityEngine;


namespace TopicTwister.Shared.Utils
{
    public class BotAnswerDtosGenerator : IBotAnswerDtosGenerator
    {
        private string _path = $"{Application.dataPath}/Resources/JSON/DevelopmentData/Words.json";
        private List<WordDaoJson> _words;

        public List<AnswerDto> GenerateAnswers(List<AnswerDto> userAnswerDtos, char initialLetter)
        {
            string data = File.ReadAllText(_path);
            _words = new WordDaosCollectionDeserializer().Deserialize(data).Words;

            bool willAnswerCorrectly;
            AnswerDto botAnswer;
            List<AnswerDto> generatedAnswers = new List<AnswerDto>();

            foreach (AnswerDto answerDto in userAnswerDtos)
            {
                List<string> words = new List<string>();
                foreach (var word in _words)
                {
                    if (word.CategoryId == answerDto.CategoryDto.Id && word.Text[0] == initialLetter) words.Add(word.Text);
                }

                willAnswerCorrectly = UnityEngine.Random.value > 0.5f;
                if (willAnswerCorrectly)
                {
                    botAnswer = new AnswerDto(
                        categoryDto: answerDto.CategoryDto,
                        userInput: words[0].ToUpper(),
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
