using System;
using System.Collections.Generic;
using System.Linq;

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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            RoundWithCategoriesDto other = (RoundWithCategoriesDto)obj;

            bool roundDtoEqual = _roundDto.Equals(other._roundDto);
            bool categoryDtosEquals = Enumerable.SequenceEqual(_categoryDtos, other._categoryDtos);

            return roundDtoEqual && categoryDtosEquals;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_roundDto.GetHashCode(), _categoryDtos.GetHashCode());
        }
    }
}
