using TopicTwister.Shared.DTOs;
using TopicTwister.NewRound.Shared.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.Shared.Utils;


namespace TopicTwister.NewRound.Repositories
{
    public class CategoriesReadOnlyRepositoryJson : ICategoriesReadOnlyRepository
    {
        private const string CategoryResourceName = "JSON/DevelopmentData/Category";

        private readonly List<CategoryDto> _readCache;

        public CategoriesReadOnlyRepositoryJson()
        {
            string data = Resources.Load<TextAsset>(CategoryResourceName).text;

            _readCache = JsonUtility.FromJson<CategoriesCollection>(data).Categories;
        }

        public Result<List<CategoryDto>> GetRandomCategories(int numberOfCategories)
        {
            return Result<List<CategoryDto>>.Success(
                outcome: _readCache
                .OrderBy(c => new System.Random()
                .Next())
                .Take(numberOfCategories)
                .ToArray());
        }

        public Result<bool> Exists(string name)
        {
            return _readCache.Any(catgory => catgory.Name == name);
        }

        public Result<bool> Exists(string[] names)
        {
            return names.All(name => Exists(name));
        }
    }
}
