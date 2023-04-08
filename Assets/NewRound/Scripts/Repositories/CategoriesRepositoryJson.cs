using TopicTwister.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TopicTwister.NewRound.Repositories
{
    public class CategoriesRepositoryJson : ICategoryRepository
    {
        private const string CategoryResourceName = "Category";

        private readonly List<CategoryDTO> _categories;

        public CategoriesRepositoryJson()
        {
            string data = Resources.Load<TextAsset>(CategoryResourceName).text;
        
            _categories = JsonUtility.FromJson<CategoriesCollection>(data).Categories;
        }

        public CategoryDTO[] GetRandomCategories(int numberOfCategories)
        {
            return _categories.OrderBy(c => new System.Random().Next()).Take(numberOfCategories).ToArray();
        }

        public bool Exists(string name)
        {
            return _categories.Any(catgory => catgory.Name == name);
        }

        public bool Exists(string[] names)
        {
            return names.All(name => Exists(name));
        }
    }
}
