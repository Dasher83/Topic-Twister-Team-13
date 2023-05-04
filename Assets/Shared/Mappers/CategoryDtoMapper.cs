using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class CategoryDtoMapper : IdtoMapper<Category, CategoryDto>
    {
        public Category FromDTO(CategoryDto categoryDto)
        {
            return new Category(id: categoryDto.Id, name: categoryDto.Name);
        }

        public List<Category> FromDTOs(List<CategoryDto> categoryDtos)
        {
            return categoryDtos.Select(FromDTO).ToList();
        }

        public CategoryDto ToDTO(Category category)
        {
            return new CategoryDto(id: category.Id, name: category.Name);
        }

        public List<CategoryDto> ToDTOs(List<Category> models)
        {
            throw new System.NotImplementedException();
        }
    }
}
