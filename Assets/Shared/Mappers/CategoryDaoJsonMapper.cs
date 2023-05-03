using System.Collections.Generic;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.Shared.Mappers
{
    public class CategoryDaoJsonMapper : IdaoMapper<Category, CategoryDaoJson>
    {
        public Category FromDAO(CategoryDaoJson categoryDao)
        {
            return new Category(
                id: categoryDao.Id,
                name: categoryDao.Name);
        }

        public List<Category> FromDAOs(List<CategoryDaoJson> categoryDaos)
        {
            throw new System.NotImplementedException();
        }

        public CategoryDaoJson ToDAO(Category category)
        {
            throw new System.NotImplementedException();
        }

        public List<CategoryDaoJson> ToDAOs(List<Category> categories)
        {
            throw new System.NotImplementedException();
        }
    }
}