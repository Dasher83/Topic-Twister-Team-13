using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.Shared.DAOs;
using UnityEngine;


namespace TopicTwister.Shared.Serialization.Shared
{
    [Serializable]
    public class CategoryDaosCollection
    {
        [SerializeField] private CategoryDaoJson[] _categories;

        public List<CategoryDaoJson> Categories => _categories.ToList();
    }
}
