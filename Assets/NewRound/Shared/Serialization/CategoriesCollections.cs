using System;
using System.Collections.Generic;
using System.Linq;
using TopicTwister.NewRound.Shared.DTOs;
using UnityEngine;


namespace TopicTwister.NewRound.Shared.Serialization
{
    [Serializable]
    public class CategoriesCollection
    {
        [SerializeField] private CategoryDTO[] _categories;

        public List<CategoryDTO> Categories => _categories.ToList();
    }

}
