using System.Collections.Generic;


namespace TopicTwister.Shared.DTOs
{
    public class RoundWithCategoriesDto
    {
        private RoundDto _roundDto;
        private List<CategoryDto> _categoryDtos;

        public RoundDto RoundDto => _roundDto;
        public List<CategoryDto> CategoryDtos => _categoryDtos;

        public RoundWithCategoriesDto(RoundDto roundDto, List<CategoryDto> categoryDtos)
        { 
            _roundDto = roundDto;
            _categoryDtos = categoryDtos;
        }
    }
}
