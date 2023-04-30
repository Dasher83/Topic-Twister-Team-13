using TopicTwister.NewRound.Shared.DAOs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Utils;
using TopicTwister.NewRound.Shared.Serialization;
using TopicTwister.Shared.Interfaces;
using TopicTwister.Shared.Models;


namespace TopicTwister.NewRound.Repositories
{
    public class CategoriesReadOnlyRepositoryJson : ICategoriesReadOnlyRepository
    {
        private readonly IdaoMapper<Category, CategoryDao> _mapper;

        private string _categoryResourceName;
        private readonly List<CategoryDao> _readCache;

        public CategoriesReadOnlyRepositoryJson(string categoriesResourceName, IdaoMapper<Category, CategoryDao> mapper)
        {
            _categoryResourceName = categoriesResourceName;
            string data = Resources.Load<TextAsset>($"JSON/{_categoryResourceName}").text;
            _readCache = JsonUtility.FromJson<CategoryDaoCollection>(data).Categories;
            _mapper = mapper;
        }

        public Result<List<Category>> GetRandomCategories(int numberOfCategories)
        {
            List<Category> randomCategories = _readCache
                .OrderBy(categoryDao => new System.Random().Next())
                .Take(numberOfCategories)
                .Select(categoryDao => _mapper.FromDAO(categoryDao))
                .ToList();

            return Result<List<Category>>.Success(outcome: randomCategories);
        }

        public Result<bool> Exists(string name)
        {
            return Result<bool>.Success(outcome: _readCache.Any(catgory => catgory.Name == name));
        }

        public Result<bool> Exists(string[] names)
        {
            return Result<bool>.Success(outcome: names.All(name => Exists(name).Outcome));
        }
    }
}
