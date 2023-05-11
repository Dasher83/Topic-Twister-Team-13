using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TopicTwister.Shared.Utils;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;
using TopicTwister.Shared.DAOs;
using TopicTwister.Shared.Serialization.Shared;


namespace TopicTwister.Shared.Repositories
{
    public class CategoriesReadOnlyRepositoryJson : ICategoriesReadOnlyRepository
    {
        private readonly IdaoMapper<Category, CategoryDaoJson> _categoryDaoMapper;

        private string _resourceName;
        private readonly List<CategoryDaoJson> _readCache;

        public CategoriesReadOnlyRepositoryJson(
            string resourceName,
            IdaoMapper<Category, CategoryDaoJson> categoryDaoJsonMapper)
        {
            _resourceName = resourceName;
            string data = Resources.Load<TextAsset>($"JSON/{_resourceName}").text;
            _readCache = JsonUtility.FromJson<CategoryDaosCollection>(data).Categories;
            _categoryDaoMapper = categoryDaoJsonMapper;
        }

        public Operation<List<Category>> GetRandomCategories(int numberOfCategories)
        {
            List<Category> randomCategories = _readCache
                .OrderBy(categoryDao => new System.Random().Next())
                .Take(numberOfCategories)
                .Select(categoryDao => _categoryDaoMapper.FromDAO(categoryDao))
                .ToList();

            return Operation<List<Category>>.Success(result: randomCategories);
        }

        public Operation<List<Category>> GetMany(List<int> categoryIds)
        {
            List<Category> filteredCategories = _readCache
                .Where(categoryDao => categoryIds.Contains(categoryDao.Id))
                .Distinct()
                .Select(categoryDao => _categoryDaoMapper.FromDAO(categoryDao))
                .ToList();

            List<int> idsFound = filteredCategories.Select(category => category.Id).ToList();
            List<int> notFoundIds = categoryIds.Intersect(second: idsFound).ToList();

            if(filteredCategories.Count != categoryIds.Count)
            {
                Operation<List<Category>>.Failure(
                    errorMessage: $"Categories not found with ids: [{string.Join(", ", notFoundIds)}]");
            }

            return Operation<List<Category>>.Success(result: filteredCategories);
        }

        public Operation<bool> Exists(string name)
        {
            return Operation<bool>.Success(result: _readCache.Any(catgory => catgory.Name == name));
        }

        public Operation<bool> Exists(string[] names)
        {
            return Operation<bool>.Success(result: names.All(name => Exists(name).Result));
        }

        public Operation<Category> Get(int id)
        {
            CategoryDaoJson categorydao = _readCache
                .SingleOrDefault(categoryDao => categoryDao.Id == id);

            if (categorydao == null)
            {
                Operation<List<Category>>.Failure(
                    errorMessage: $"Category not found with id: {id}");
            }

            Category category = _categoryDaoMapper.FromDAO(categorydao);

            return Operation<Category>.Success(result: category);
        }
    }
}
