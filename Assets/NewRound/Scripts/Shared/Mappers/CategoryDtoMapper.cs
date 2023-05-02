using System.Collections.Generic;
using TopicTwister.Shared.DTOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.NewRound.Shared.Mappers
{
    public class CategoryDtoMapper : IdtoMapper<Category, CategoryDto>
    {
        public Category FromDTO(CategoryDto DTO)
        {
            throw new System.NotImplementedException();
        }

        public List<Category> FromDTOs(List<CategoryDto> DTOs)
        {
            throw new System.NotImplementedException();
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