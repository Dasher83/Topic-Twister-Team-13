using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TopicTwister.NewRound.Shared.Serialization
{
    [Serializable]
    public class CategoryDaoCollection
    {
        [SerializeField] private CategoryDao[] _categories;

        public List<CategoryDao> Categories => _categories.ToList();
    }

}
