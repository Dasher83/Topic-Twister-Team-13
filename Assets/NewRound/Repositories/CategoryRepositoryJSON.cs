using System.Collections.Generic;
using TopicTwister.NewRound.Shared.DTOs;
using TopicTwister.NewRound.Shared.Interfaces;
using TopicTwister.NewRound.Shared.Serialization;
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
            return _categories.ToArray();
        }
    }
}
