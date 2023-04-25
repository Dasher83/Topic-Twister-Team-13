using System.Collections.Generic;


namespace TopicTwister.Shared.DTOs
{
    public class RoundWithCategoriesDto
    {
        private RoundDto _roundDto;
        private List<CategoryDTO> _categoryDtos;

        public RoundDto RoundDto => _roundDto;
        public List<CategoryDTO> CategoryDtos => _categoryDtos;

        public RoundWithCategoriesDto(RoundDto roundDto, List<CategoryDTO> categoryDtos)
        { 
            _roundDto = roundDto;
            _categoryDtos = categoryDtos;
        }
    }
}
