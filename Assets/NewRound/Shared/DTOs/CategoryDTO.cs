using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TopicTwister.NewRound.Shared.DTOs
{
    [Serializable] 
    public class CategoriesCollection 
    {
        [SerializeField]
        private CategoryDTO[] _categories;

        public List<CategoryDTO> Categories => _categories.ToList();
    }
    
    
    
    [Serializable]
    public class CategoryDTO
    {
        [SerializeField]
        private string _id;
        
        [SerializeField]
        private string _name;

        public string Id => _id;
        public string Name => _name;

        private CategoryDTO() { }

        public CategoryDTO(string id, string name)
        {
            _id = id;
            _name = name;
        }

        public override bool Equals(object obj)
        {
            CategoryDTO other = obj as CategoryDTO;

            if(this._id != other._id) return false;
            if(this._name != other._name) return false;
            return true;
        }
    }
}
