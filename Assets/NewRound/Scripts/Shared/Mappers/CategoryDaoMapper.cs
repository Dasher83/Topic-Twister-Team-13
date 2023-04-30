using System.Collections.Generic;
using TopicTwister.NewRound.Shared.DAOs;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.NewRound.Shared.Mappers
{
    public class CategoryDaoMapper : IdaoMapper<Category, CategoryDao>
    {
        public Category FromDAO(CategoryDao categoryDao)
        {
            return new Category(
                id: categoryDao.Id,
                name: categoryDao.Name);
        }

        public List<Category> FromDAOs(List<CategoryDao> categoryDaos)
        {
            throw new System.NotImplementedException();
        }

        public CategoryDao ToDAO(Category category)
        {
            throw new System.NotImplementedException();
        }

        public List<CategoryDao> ToDAOs(List<Category> categories)
        {
            throw new System.NotImplementedException();
        }
    }
}