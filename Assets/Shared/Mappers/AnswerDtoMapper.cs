using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class AnswerDtoMapper : IdtoMapper<Answer, AnswerDto>
    {
        private IdtoMapper<Category, CategoryDto> _categoryDtoMapper;
        private IWordsRepository _wordsRepository;

        private AnswerDtoMapper() { }

        public AnswerDtoMapper(IdtoMapper<Category, CategoryDto> categoryDtoMapper, IWordsRepository wordsRepository)
        {
            _categoryDtoMapper = categoryDtoMapper;
            _wordsRepository = wordsRepository;
        }

        public Answer FromDTO(AnswerDto DTO)
        {
            throw new System.NotImplementedException();
        }

        public List<Answer> FromDTOs(List<AnswerDto> DTOs)
        {
            throw new System.NotImplementedException();
        }

        public AnswerDto ToDTO(Answer answer)
        {
            AnswerDto answerDto = new AnswerDto(
                categoryDto: _categoryDtoMapper.ToDTO(answer.Category),
                userInput: answer.UserInput,
                order: answer.Order,
                isCorrect: answer.IsCorrect(wordsRepository: _wordsRepository));

            return answerDto;
        }

        public List<AnswerDto> ToDTOs(List<Answer> answers)
        {
            return answers.Select(ToDTO).ToList();
        }
    }
}
