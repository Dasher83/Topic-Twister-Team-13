using System.Collections.Generic;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;


namespace TopicTwister.Shared.Mappers
{
    public class RoundWithCategoriesDtoMapper : IdtoMapper<Round, RoundWithCategoriesDto>
    {
        private IdtoMapper<Category, CategoryDto> _categoryDtoMapper;
        private IdtoMapper<Round, RoundDto> _roundDtoMapper;
        private IRoundsReadOnlyRepository _roundReadOnlyRepository;

        public RoundWithCategoriesDtoMapper(
            IdtoMapper<Category, CategoryDto> categoryDtoMapper,
            IdtoMapper<Round, RoundDto> roundDtoMapper,
            IRoundsReadOnlyRepository roundReadOnlyRepository)
        {
            _categoryDtoMapper = categoryDtoMapper;
            _roundDtoMapper = roundDtoMapper;
            _roundReadOnlyRepository = roundReadOnlyRepository;
        }

        public Round FromDTO(RoundWithCategoriesDto dto)
        {
            return _roundReadOnlyRepository.Get(dto.RoundDto.Id).Outcome;
        }

        public List<Round> FromDTOs(List<RoundWithCategoriesDto> dtos)
        {
            throw new System.NotImplementedException();
        }

        public RoundWithCategoriesDto ToDTO(Round round)
        {
            RoundDto roundDto = _roundDtoMapper.ToDTO(round);

            List<CategoryDto> categoryDtos = new List<CategoryDto>();
            foreach(Category category in round.Categories)
            {
                categoryDtos.Add(_categoryDtoMapper.ToDTO(category));
            }

            RoundWithCategoriesDto roundWithCategoriesDto = new RoundWithCategoriesDto(roundDto, categoryDtos);

            return roundWithCategoriesDto;
        }

        public List<RoundWithCategoriesDto> ToDTOs(List<Round> rounds)
        {
            throw new System.NotImplementedException();
        }
    }
}
