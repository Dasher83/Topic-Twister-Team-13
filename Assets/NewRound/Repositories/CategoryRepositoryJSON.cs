using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TopicTwister.NewRound.Repositories
{
    public class CategoryRepositoryJSON : ICategoryRepository
    {
        private const string CategoryResourceName = "Category";

        private readonly List<CategoryDTO> _categories;

        public CategoryRepositoryJSON()
        {
            string data = Resources.Load<TextAsset>(CategoryResourceName).text;
        
            _categories = JsonUtility.FromJson<CategoriesCollection>(data).Categories;
        }

        public CategoryDTO[] GetRandomCategories(int numberOfCategories)
        {
            return _categories.Take(numberOfCategories).ToArray();
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
